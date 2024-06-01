using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Parkomat.Data;
using Parkomat.Models;
using System.Diagnostics;

namespace Parkomat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
                parking.Cost += cost[Math.Min(pointer, 3)]*time/60;

            }
            else
            {
                return View();
            }
                
            _context.Parkings.Add(parking);
            _context.SaveChanges();
            return View();
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
                .Include(p=>p.ParkingLot)
                .ToListAsync();
            return View(parkings);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
