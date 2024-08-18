using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Parkomat.Data;
using Parkomat.Models;
using System.Diagnostics;

namespace Parkomat.Controllers
{
    /// <summary>
    /// Controller responsible for handling the home page and error page of the application.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The view of the home page.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page with details of the current request.
        /// </summary>
        /// <returns>The view of the error page with the request ID.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
