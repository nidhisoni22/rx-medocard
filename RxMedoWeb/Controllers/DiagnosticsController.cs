using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RxMedoWeb.Data;
using RxMedoWeb.Models;

namespace RxMedoWeb.Controllers
{
    public class DiagnosticsController : Controller
    {
        private readonly ILogger<DiagnosticsController> _logger;
        private readonly ApplicationDbContext _context;

        public DiagnosticsController(ILogger<DiagnosticsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var diagnosticTests = GetDiagnosticTests();
            return View(diagnosticTests);
        }

        [HttpPost]
        public async Task<IActionResult> BookTest(DiagnosticBooking booking)
        {
            // Log the booking request
            _logger.LogInformation("Received diagnostic test booking request");

            // Check if booking is null
            if (booking == null)
            {
                _logger.LogError("Booking object is null - model binding failed");
                TempData["ErrorMessage"] = "There was an error processing your form. Please try again.";
                return RedirectToAction("Index");
            }

            // Check model state and log any validation errors
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState validation failed for diagnostic booking");

                TempData["ErrorMessage"] = "Please fill out all required fields correctly.";
                return RedirectToAction("Index");
            }

            try
            {
                // Set booking date to current date and time
                booking.BookingDate = DateTime.Now;
                booking.IsConfirmed = false;

                // Log booking details before saving
                _logger.LogInformation("Processing diagnostic test booking");

                // Add the booking to the database
                _context.DiagnosticBookings.Add(booking);

                // Save to database
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Diagnostic booking saved to database");

                if (result > 0)
                {
                    // Verify the booking was saved by retrieving it
                    var savedBooking = await _context.DiagnosticBookings.FindAsync(booking.Id);
                    if (savedBooking != null)
                    {
                        _logger.LogInformation("Booking verification successful");

                        // Check if this is an AJAX request
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                        {
                            return Json(new { success = true, message = "Booking saved successfully", bookingId = savedBooking.Id });
                        }
                        else
                        {
                            // Redirect to the same page with a success message for regular form submissions
                            TempData["SuccessMessage"] = "Your diagnostic test booking has been confirmed. Our team will contact you shortly with appointment details.";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to retrieve the saved booking");
                    }
                }
                else
                {
                    _logger.LogWarning("SaveChangesAsync returned 0 records affected");
                }

                // If we got here, something went wrong with the save
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return Json(new { success = false, message = "Failed to save booking" });
                }
                else
                {
                    TempData["ErrorMessage"] = "There was an error saving your booking. Please try again.";
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error saving diagnostic booking");
                if (dbEx.InnerException != null)
                {
                    _logger.LogError("Database inner exception detected");
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return Json(new { success = false, message = $"Database error: {dbEx.Message}" });
                }
                else
                {
                    TempData["ErrorMessage"] = $"Database error: {dbEx.Message}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing diagnostic booking");
                if (ex.InnerException != null)
                {
                    _logger.LogError("General exception inner exception detected");
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return Json(new { success = false, message = "An error occurred while booking your test. Please try again." });
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while booking your test. Please try again.";
                    return RedirectToAction("Index");
                }
            }
        }

        // Admin page to view all bookings
        public async Task<IActionResult> Bookings()
        {
            try
            {
                // Get all bookings
                var bookings = await _context.DiagnosticBookings.OrderByDescending(b => b.BookingDate).ToListAsync();
                return View(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings");
                TempData["ErrorMessage"] = "There was an error retrieving the bookings. Please try again.";
                return View(new List<DiagnosticBooking>());
            }
        }

        private List<DiagnosticTest> GetDiagnosticTests()
        {
            // In a real application, these would come from a database
            return new List<DiagnosticTest>
            {
                new DiagnosticTest { Id = 1, Name = "Complete Blood Count (CBC)", OriginalPrice = 1200, DiscountedPrice = 840, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 2, Name = "Lipid Profile", OriginalPrice = 1500, DiscountedPrice = 1050, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 3, Name = "Liver Function Test", OriginalPrice = 1800, DiscountedPrice = 1260, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 4, Name = "Kidney Function Test", OriginalPrice = 1600, DiscountedPrice = 1120, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 5, Name = "Blood Glucose Test", OriginalPrice = 500, DiscountedPrice = 350, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 6, Name = "Thyroid Profile", OriginalPrice = 1800, DiscountedPrice = 1260, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 7, Name = "X-Ray", OriginalPrice = 2000, DiscountedPrice = 1400, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 8, Name = "Ultrasound", OriginalPrice = 2500, DiscountedPrice = 1750, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 9, Name = "MRI", OriginalPrice = 8000, DiscountedPrice = 5600, DiscountPercentage = 30 },
                new DiagnosticTest { Id = 10, Name = "CT Scan", OriginalPrice = 6000, DiscountedPrice = 4200, DiscountPercentage = 30 }
            };
        }
    }
}
