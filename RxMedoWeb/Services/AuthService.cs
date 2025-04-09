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
    }
}
