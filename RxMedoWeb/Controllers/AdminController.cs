using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RxMedoWeb.Data;
using RxMedoWeb.Models;
using RxMedoWeb.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace RxMedoWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context, AuthService authService)
        {
            _logger = logger;
            _context = context;
            _authService = authService;
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
    }
}
