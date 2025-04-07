using Microsoft.AspNetCore.Mvc;
using RxMedoWeb.Models;

namespace RxMedoWeb.Controllers
{
    public class MembershipController : Controller
    {
        private readonly ILogger<MembershipController> _logger;

        public MembershipController(ILogger<MembershipController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Apply(Membership membership)
        {
            if (ModelState.IsValid)
            {
                // In a real application, save the membership application to a database
                _logger.LogInformation($"Membership application received from {membership.FullName}");

                // Generate a random membership number (in a real app, this would be more sophisticated)
                membership.MembershipNumber = "RX" + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 9999).ToString();

                // Redirect to the same page with a success message
                TempData["SuccessMessage"] = $"Your membership application has been submitted successfully. Your temporary membership number is {membership.MembershipNumber}. We will contact you shortly to complete the process.";
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View("Index", membership);
        }
    }
}
