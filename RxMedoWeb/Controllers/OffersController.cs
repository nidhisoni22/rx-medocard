using Microsoft.AspNetCore.Mvc;

namespace RxMedoWeb.Controllers
{
    public class OffersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
