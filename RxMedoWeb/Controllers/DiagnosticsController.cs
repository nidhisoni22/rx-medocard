using Microsoft.AspNetCore.Mvc;
using RxMedoWeb.Models;

namespace RxMedoWeb.Controllers
{
    public class DiagnosticsController : Controller
    {
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(ILogger<DiagnosticsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // In a real application, these would come from a database
            var diagnosticTests = new List<DiagnosticTest>
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

            return View(diagnosticTests);
        }

        [HttpPost]
        public IActionResult BookTest(DiagnosticBooking booking)
        {
            if (ModelState.IsValid)
            {
                // In a real application, save the booking to a database
                _logger.LogInformation($"Diagnostic test booked for {booking.PatientName}");

                // Redirect to the same page with a success message
                TempData["SuccessMessage"] = "Your diagnostic test has been booked successfully. We will contact you shortly to confirm the appointment.";
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            // In a real application, we would need to pass the diagnostic tests list back to the view
            return RedirectToAction("Index");
        }
    }
}
