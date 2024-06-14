using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        public IActionResult CreateParking(string carLicensePlate, int time)
        {
            var parking = new Parking()
            {
                ParkingStop = DateTime.Now.AddMinutes(time),
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
                var timeInMinutes = time;
                var pointer = 0;
                decimal[] cost = { pricelist.Hour1, pricelist.Hour2, pricelist.Hour3, pricelist.Rest };
                while (timeInMinutes > 60)
                {
                    parking.Cost += cost[Math.Min(pointer, 3)];
                    timeInMinutes -= 60;
                    pointer++;
                }
                parking.Cost += cost[Math.Min(pointer, 3)] * timeInMinutes / 60;

            }
            else
            {
                return View();
            }

            _context.Parkings.Add(parking);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [Authorize(Roles = SD.Role_User)]
        [Route("Parking/Premium")]
        public IActionResult ParkingPremium()
        {
            var userId = _userManager.GetUserId(User);
            if(userId == null)
            {
                return NotFound("Nie znaleziono użytkownika");
            }
            var parking = _context.Parkings
                .FirstOrDefault(p => p.UserId == userId && p.ParkingStop == null);
            if (parking == null)
            {
                return View(new Parking());
            }
            else
            {
                return View(parking);
            }
        }

        [Authorize(Roles = SD.Role_User)]
        [HttpPost]
        [ActionName("ParkingPremiumStart")]
        public IActionResult ParkingPremiumStartAction(string carLicensePlate)
        {
            var parking = new Parking()
            {
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

            _context.Parkings.Add(parking);
            _context.SaveChanges();
            int id = parking.ParkingId;

            var viewModel = new Parking
            {
                ParkingId = id,
                CarLicensePlate = parking.CarLicensePlate,
                ParkingStart = parking.ParkingStart,
            };

            return View("ParkingPremium", viewModel);
        }

        [Authorize(Roles = SD.Role_User)]
        [HttpPost]
        [ActionName("ParkingPremiumStop")]
        public IActionResult ParkingPremiumStopAction(int ParkingId)
        {
            int id = ParkingId;
            Console.WriteLine(id);
            var parking = _context.Parkings.FirstOrDefault(p => p.ParkingId == ParkingId);


            parking.ParkingStop = DateTime.Now;
            
            var parkinglot = _context.ParkingsLots.FirstOrDefault(x => x.ParkingLotId == parking.ParkingLotID);
            if (parkinglot != null)
            {
                var pricelist = _context.PriceLists.FirstOrDefault(x => x.PriceListId == parkinglot.PriceListId);
                TimeSpan time = (TimeSpan)(parking.ParkingStop - parking.ParkingStart);
                int timeInMinutes = (int)time.TotalMinutes;
                var pointer = 0;
                decimal[] cost = { pricelist.Hour1, pricelist.Hour2, pricelist.Hour3, pricelist.Rest };
                while (timeInMinutes > 60)
                {
                    parking.Cost += cost[Math.Min(pointer, 3)];
                    timeInMinutes -= 60;
                    pointer++;
                }
                parking.Cost += cost[Math.Min(pointer, 3)] * timeInMinutes / 60;

            }
            else
            {
                return RedirectToAction("ParkingPremium");
            }

            _context.Parkings.Update(parking);
            _context.SaveChanges();
            return RedirectToAction("ParkingPremium");
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
