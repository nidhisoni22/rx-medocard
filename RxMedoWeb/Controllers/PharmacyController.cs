using Microsoft.AspNetCore.Mvc;

namespace RxMedoWeb.Controllers
{
    public class PharmacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
