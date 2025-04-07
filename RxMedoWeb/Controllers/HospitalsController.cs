using Microsoft.AspNetCore.Mvc;

namespace RxMedoWeb.Controllers
{
    public class HospitalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
