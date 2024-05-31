using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
