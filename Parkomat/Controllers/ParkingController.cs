using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Parkomat.Data;
using Parkomat.Models;


namespace Parkomat.Controllers
{
    /// <summary>
    /// Controller responsible for managing parking operations.
    /// </summary>
    public class ParkingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParkingController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging purposes.</param>
        /// <param name="context">The database context.</param>
        /// <param name="userManager">The user manager to handle user operations.</param>

        public ParkingController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the parking index page.
        /// </summary>
        /// <returns>The view of the index page.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the page to create a new parking instance.
        /// </summary>
        /// <returns>The view to create a new parking.</returns>
        public IActionResult CreateParking()
        {
            return View();
        }
        /// <summary>
        /// Calculates cost and adds information about parking instance to database.
        /// </summary>
        /// <param name="carLicensePlate">String containing car license plate for identification.</param>
        /// <param name="time">Int value representing minutes and used for calculating cost.</param>
        /// <returns>Refreshes page if something goes wrong and goes to home on success</returns>
        [HttpPost]
        public IActionResult CreateParking(string carLicensePlate, int time)
        {
            Console.WriteLine(time);
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
            return RedirectToAction("index" , "Home");
        }

        /// <summary>
        /// Checks for active parkings and returns the subpage accordingly
        /// </summary>
        /// <returns>Returns notfound subpage if user id is not found. Returns subpage with ongoing parking information or creation subpage if there are none</returns>
        [Authorize(Roles = SD.Role_User)]
        [Route("Parking/Premium")]
        public IActionResult ParkingPremium()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
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
        /// <summary>
        /// Enters information about parking into the database.
        /// </summary>
        /// <param name="carLicensePlate">String containing car license plate for identification.</param>
        /// <returns>Returns subpage with parking information.</returns>
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

        /// <summary>
        /// Calculates time spent parking and its cost then updates database with these values.
        /// </summary>
        /// <param name="ParkingId">Int containing identification number of parkings.</param>
        /// <returns>Redirects to the ParkingPremium action after updating the database.</returns>
        [Authorize(Roles = SD.Role_User)]
        [HttpPost]
        [ActionName("ParkingPremiumStop")]
        public IActionResult ParkingPremiumStopAction(int ParkingId)
        {
            int id = ParkingId;
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

        /// <summary>
        /// Retrieves the parking history for the logged-in user.
        /// </summary>
        /// <returns>Subpage that contains the view of the parking history.</returns>
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

        /// <summary>
        /// Retrieves the parking history for all users (admin only).
        /// </summary>
        /// <returns>Subpage that contains list of all users.</returns>
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> AdminHistory()
        {
            var users = await _context.ApplicationUsers.ToListAsync();
            return View(users);
        }

        /// <summary>
        /// Displays the parking history for a specific user by email (admin only).
        /// </summary>
        /// <param name="mail">String containing the email address of the user whose parking history is to be displayed.</param>
        /// <returns>Subpage that contains the view of the parking history for the specified user.</returns>
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> AdminHistoryDisplay(string mail)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Email == mail);
            var parkings = await _context.Parkings
                .Where(p => p.UserId == user.Id)
                .Include(p => p.ParkingLot)
                .ToListAsync();
            return View(parkings);
        }
    }
}
