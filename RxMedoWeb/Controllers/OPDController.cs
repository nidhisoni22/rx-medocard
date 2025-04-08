using Microsoft.AspNetCore.Mvc;

namespace RxMedoWeb.Controllers
{
    public class OPDController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
