using Microsoft.AspNetCore.Mvc;
using RxMedoWeb.Data;
using RxMedoWeb.Models;

namespace RxMedoWeb.Controllers
{
    public class MembershipController : Controller
    {
        private readonly ILogger<MembershipController> _logger;
        private readonly ApplicationDbContext _context;

        public MembershipController(ILogger<MembershipController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
                // Generate a random membership number
                membership.MembershipNumber = "RX" + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 9999).ToString();

                // Set application date to current date and time
                membership.ApplicationDate = DateTime.Now;

                // Save the membership application to the database
                _context.Memberships.Add(membership);
                _context.SaveChanges();

                _logger.LogInformation($"Membership application saved to database for {membership.FullName} with ID {membership.Id}");

                // Redirect to the same page with a success message
                TempData["SuccessMessage"] = $"Your membership application has been submitted successfully. Your temporary membership number is {membership.MembershipNumber}. We will contact you shortly to complete the process.";
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View("Index", membership);
        }
    }
}
