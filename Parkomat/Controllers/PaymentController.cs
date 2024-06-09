using Microsoft.AspNetCore.Mvc;

namespace Parkomat.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
    }
}
