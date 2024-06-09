using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Parkomat.Data;
using Parkomat.Models;

namespace Parkomat.Controllers
{
    public class ParkingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ParkingController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateParking()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateParking(string carLicensePlate, int timeInMinutes)
        {
            var parking = new Parking()
            {
                ParkingStop = DateTime.Now.AddMinutes(timeInMinutes),
                ParkingStart = DateTime.Now,
                CarLicensePlate = carLicensePlate,
                ParkingLotID = 1,
                UserId = _userManager.GetUserId(User),
                Cost = 0
            };
            if (parking.UserId.IsNullOrEmpty())
            {
                parking.UserId = "unlogged";
            }
            var parkinglot = _context.ParkingsLots.FirstOrDefault(x => x.ParkingLotId == parking.ParkingLotID);
            if (parkinglot != null)
            {
                var pricelist = _context.PriceLists.FirstOrDefault(x => x.PriceListId == parkinglot.PriceListId);
                var time = timeInMinutes;
                var pointer = 0;
                decimal[] cost = { pricelist.Hour1, pricelist.Hour2, pricelist.Hour3, pricelist.Rest };
                while (time > 60)
                {
                    parking.Cost += cost[Math.Min(pointer, 3)];
                    time -= 60;
                    pointer++;
                }
                parking.Cost += cost[Math.Min(pointer, 3)] * time / 60;

            }
            else
            {
                return View();
            }

            _context.Parkings.Add(parking);
            //_context.SaveChanges();
            return RedirectToAction("Payment");
        }

        [Authorize(Roles = SD.Role_User)]
        public IActionResult ParkingPremium()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_User)]
        public async Task<IActionResult> ParkingHistory()
        {
            var userId = _userManager.GetUserId(User);
            var parkings = await _context.Parkings
                .Where(p => p.UserId == userId)
                .Include(p => p.ParkingLot)
                .ToListAsync();
            return View(parkings);
        }
    }
}
