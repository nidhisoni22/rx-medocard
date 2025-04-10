using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RxMedoWeb.Data;
using RxMedoWeb.Models;
using RxMedoWeb.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace RxMedoWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        private readonly EmailService _emailService;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context, AuthService authService, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            // If user is already authenticated, redirect to dashboard
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isValid = await _authService.ValidateUserAsync(model.Username, model.Password);

                if (isValid)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3) // Session expires after 3 hours
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation("User {Username} logged in", model.Username);
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Get counts for dashboard
                var diagnosticBookingsCount = await _context.DiagnosticBookings.CountAsync();
                var membershipsCount = await _context.Memberships.CountAsync();

                // Get recent bookings and memberships
                var recentDiagnosticBookings = await _context.DiagnosticBookings
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToListAsync();

                var recentMemberships = await _context.Memberships
                    .OrderByDescending(m => m.ApplicationDate)
                    .Take(5)
                    .ToListAsync();

                // Pass data to view using ViewBag
                ViewBag.DiagnosticBookingsCount = diagnosticBookingsCount;
                ViewBag.MembershipsCount = membershipsCount;
                ViewBag.RecentDiagnosticBookings = recentDiagnosticBookings;
                ViewBag.RecentMemberships = recentMemberships;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard");
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DiagnosticBookings()
        {
            try
            {
                var bookings = await _context.DiagnosticBookings
                    .OrderByDescending(b => b.BookingDate)
                    .ToListAsync();

                return View(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagnostic bookings");
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Memberships()
        {
            try
            {
                var memberships = await _context.Memberships
                    .OrderByDescending(m => m.ApplicationDate)
                    .ToListAsync();

                return View(memberships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving memberships");
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveDiagnosticBooking(int id)
        {
            try
            {
                var booking = await _context.DiagnosticBookings.FindAsync(id);

                if (booking == null)
                {
                    return NotFound();
                }

                booking.IsConfirmed = true;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Diagnostic booking {Id} approved by admin", id);

                TempData["SuccessMessage"] = "Diagnostic booking approved successfully.";
                return RedirectToAction(nameof(DiagnosticBookings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving diagnostic booking {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while approving the booking.";
                return RedirectToAction(nameof(DiagnosticBookings));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteDiagnosticBooking(int id)
        {
            try
            {
                var booking = await _context.DiagnosticBookings.FindAsync(id);

                if (booking == null)
                {
                    return NotFound();
                }

                _context.DiagnosticBookings.Remove(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Diagnostic booking {Id} deleted by admin", id);

                TempData["SuccessMessage"] = "Diagnostic booking deleted successfully.";
                return RedirectToAction(nameof(DiagnosticBookings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting diagnostic booking {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the booking.";
                return RedirectToAction(nameof(DiagnosticBookings));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveMembership(int id)
        {
            try
            {
                var membership = await _context.Memberships.FindAsync(id);

                if (membership == null)
                {
                    return NotFound();
                }

                membership.IsApproved = true;

                // If membership number is not set, generate one
                if (string.IsNullOrEmpty(membership.MembershipNumber))
                {
                    membership.MembershipNumber = "RX" + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 9999).ToString();
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Membership {Id} approved by admin", id);

                TempData["SuccessMessage"] = "Membership approved successfully.";
                return RedirectToAction(nameof(Memberships));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving membership {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while approving the membership.";
                return RedirectToAction(nameof(Memberships));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            try
            {
                var membership = await _context.Memberships.FindAsync(id);

                if (membership == null)
                {
                    return NotFound();
                }

                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Membership {Id} deleted by admin", id);

                TempData["SuccessMessage"] = "Membership deleted successfully.";
                return RedirectToAction(nameof(Memberships));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting membership {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the membership.";
                return RedirectToAction(nameof(Memberships));
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: Admin/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Admin/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Generate password reset token
            var (success, message) = await _authService.GeneratePasswordResetTokenAsync(model.Email);

            if (success)
            {
                // Get the user to send the email
                var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

                if (user != null && !string.IsNullOrEmpty(user.ResetToken))
                {
                    // Create reset link
                    var tokenBytes = Encoding.UTF8.GetBytes(user.ResetToken);
                    var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

                    var callbackUrl = Url.Action(
                        "ResetPassword",
                        "Admin",
                        new { email = model.Email, token = tokenEncoded },
                        protocol: Request.Scheme);

                    // Send email with reset link
                    var emailSubject = "Reset Your RX Medo Card Admin Password";
                    var emailBody = $@"<p>Hello,</p>
                        <p>You have requested to reset your RX Medo Card admin password.</p>
                        <p>Please click the link below to set a new password:</p>
                        <p><a href='{callbackUrl}'>Reset Password</a></p>
                        <p>If you did not request this, please ignore this email.</p>
                        <p>This link will expire in 24 hours.</p>
                        <p>Thank you,<br>RX Medo Card Team</p>";

                    var emailSent = await _emailService.SendEmailAsync(model.Email, emailSubject, emailBody);

                    if (!emailSent)
                    {
                        // If email sending fails, log the reset URL instead
                        _logger.LogWarning("Failed to send password reset email. Reset URL: {ResetUrl}", callbackUrl);
                    }
                }
            }

            // Always show success message even if email doesn't exist (security best practice)
            TempData["SuccessMessage"] = message;
            return View();
        }

        // GET: Admin/ResetPassword
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        // POST: Admin/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Decode the token
                byte[] tokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                string token = Encoding.UTF8.GetString(tokenBytes);

                // Reset the password
                var success = await _authService.ResetPasswordAsync(model.Email, token, model.Password);

                if (success)
                {
                    TempData["SuccessMessage"] = "Your password has been reset successfully. You can now log in with your new password.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid or expired password reset link. Please try again.";
                    return RedirectToAction("ForgotPassword");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", model.Email);
                TempData["ErrorMessage"] = "An error occurred while resetting your password. Please try again.";
                return RedirectToAction("ForgotPassword");
            }
        }

        // GET: Admin/AdminUsers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUsers()
        {
            try
            {
                var adminUsers = await _authService.GetAllAdminUsersAsync();

                var model = new AdminUserListViewModel
                {
                    AdminUsers = adminUsers
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving admin users");
                TempData["ErrorMessage"] = "An error occurred while retrieving admin users.";
                return RedirectToAction("Dashboard");
            }
        }

        // GET: Admin/CreateAdmin
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // POST: Admin/CreateAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var (success, message) = await _authService.CreateAdminUserAsync(model.Username, model.Email, model.Password);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("AdminUsers");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating admin user");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the admin user.");
                return View(model);
            }
        }

        // POST: Admin/ToggleAdminStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleAdminStatus(int id)
        {
            try
            {
                var (success, message) = await _authService.ToggleAdminStatusAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                }
                else
                {
                    TempData["ErrorMessage"] = message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling admin status for ID: {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the admin user status.";
            }

            return RedirectToAction("AdminUsers");
        }
    }
}
