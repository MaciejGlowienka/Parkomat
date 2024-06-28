using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parkomat.Data;
using Parkomat.Models;

namespace Parkomat.Controllers
{
    public class PaymentController : Controller
    {


        private readonly IBraintreeService _braintreeService;
        private readonly ApplicationDbContext _context;


        public PaymentController(IBraintreeService braintreeService, ApplicationDbContext context)
        {
            _braintreeService = braintreeService;
            _context = context;
        }
        public IActionResult Payment(int id)
        {
            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();  //Genarate a token
            ViewBag.ClientToken = clientToken;

            var parking = _context.Parkings.FirstOrDefault(p => p.ParkingId == id);

            var data = new ParkingVM
            {
                ParkingId = parking.ParkingId,
                Cost = parking.Cost,
                Nonce = ""
            };

            if (parking.Payed == null)
            {
                return View(data);
            }
            else return View();
        }

        [HttpPost]
        public IActionResult Success(int id)
        {
            var parking = _context.Parkings.FirstOrDefault(p => p.ParkingId == id);
            parking.Payed = true;
            _context.Parkings.Update(parking);
            _context.SaveChanges();
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
