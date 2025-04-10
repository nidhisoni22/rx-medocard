using Microsoft.EntityFrameworkCore;
using RxMedoWeb.Data;
using RxMedoWeb.Models;
using System.Security.Cryptography;
using System.Text;

namespace RxMedoWeb.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
                var user = await _context.AdminUsers
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user == null)
                {
                    _logger.LogWarning("Login attempt with invalid username: {Username}", username);
                    return false;
                }

                // Verify password
                if (VerifyPassword(password, user.PasswordHash))
                {
                    // Update last login time
                    user.LastLogin = DateTime.Now;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User {Username} logged in successfully", username);
                    return true;
                }

                _logger.LogWarning("Login attempt with invalid password for user: {Username}", username);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user {Username}", username);
                return false;
            }
        }

        public async Task<bool> CreateDefaultAdminIfNotExistsAsync()
        {
            try
            {
                // Check if any admin users exist
                if (!await _context.AdminUsers.AnyAsync())
                {
                    // Create default admin user
                    var adminUser = new AdminUser
                    {
                        Username = "admin",
                        PasswordHash = HashPassword("admin123"), // Default password
                        Email = "admin@rxmedocard.com",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    _context.AdminUsers.Add(adminUser);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Default admin user created");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating default admin user");
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }

        public async Task<(bool success, string message)> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                var user = await _context.AdminUsers
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

                if (user == null)
                {
                    _logger.LogWarning("Password reset requested for non-existent email: {Email}", email);
                    return (false, "If your email is registered, you will receive a password reset link.");
                }

                // Generate a random token
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

                // Save the token to the user record
                user.ResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(24); // Token valid for 24 hours

                await _context.SaveChangesAsync();

                _logger.LogInformation("Password reset token generated for user: {Username}", user.Username);
                return (true, "Password reset link has been sent to your email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating password reset token for email: {Email}", email);
                return (false, "An error occurred. Please try again later.");
            }
        }

        public async Task<(bool isValid, AdminUser? user)> ValidatePasswordResetTokenAsync(string email, string token)
        {
            try
            {
                var user = await _context.AdminUsers
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

                if (user == null)
                {
                    _logger.LogWarning("Password reset token validation for non-existent email: {Email}", email);
                    return (false, null);
                }

                // Check if token exists and is valid
                if (string.IsNullOrEmpty(user.ResetToken) ||
                    user.ResetToken != token ||
                    !user.ResetTokenExpiry.HasValue ||
                    user.ResetTokenExpiry.Value < DateTime.UtcNow)
                {
                    _logger.LogWarning("Invalid or expired password reset token for user: {Username}", user.Username);
                    return (false, null);
                }

                return (true, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating password reset token for email: {Email}", email);
                return (false, null);
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var (isValid, user) = await ValidatePasswordResetTokenAsync(email, token);

                if (!isValid || user == null)
                {
                    return false;
                }

                // Update password
                user.PasswordHash = HashPassword(newPassword);

                // Clear reset token
                user.ResetToken = null;
                user.ResetTokenExpiry = null;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Password reset successful for user: {Username}", user.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", email);
                return false;
            }
        }

        public async Task<List<AdminUser>> GetAllAdminUsersAsync()
        {
            try
            {
                return await _context.AdminUsers
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving admin users");
                return new List<AdminUser>();
            }
        }

        public async Task<(bool success, string message)> CreateAdminUserAsync(string username, string email, string password)
        {
            try
            {
                // Check if username already exists
                if (await _context.AdminUsers.AnyAsync(u => u.Username == username))
                {
                    return (false, "Username already exists.");
                }

                // Check if email already exists
                if (await _context.AdminUsers.AnyAsync(u => u.Email == email))
                {
                    return (false, "Email already exists.");
                }

                // Create new admin user
                var adminUser = new AdminUser
                {
                    Username = username,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.AdminUsers.Add(adminUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New admin user created: {Username}", username);
                return (true, "Admin user created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating admin user: {Username}", username);
                return (false, "An error occurred while creating the admin user.");
            }
        }

        public async Task<(bool success, string message)> ToggleAdminStatusAsync(int id)
        {
            try
            {
                var adminUser = await _context.AdminUsers.FindAsync(id);

                if (adminUser == null)
                {
                    return (false, "Admin user not found.");
                }

                // Toggle status
                adminUser.IsActive = !adminUser.IsActive;

                await _context.SaveChangesAsync();

                var statusMessage = adminUser.IsActive ? "activated" : "deactivated";
                _logger.LogInformation("Admin user {Username} {Status}", adminUser.Username, statusMessage);
                return (true, $"Admin user {statusMessage} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling admin status for ID: {Id}", id);
                return (false, "An error occurred while updating the admin user status.");
            }
        }
    }
}
