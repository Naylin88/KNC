using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InitCMS.Models;
using InitCMS.Data;
using System.Collections.Generic;
using System.Linq;
using InitCMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace InitCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InitCMSContext _context;
        private UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, InitCMSContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
                return RedirectToAction("Login", "Admin");

            }

            //product count
            var ProductCount = (from row in _context.Products
                          select row).Count();
            //Product Category Count
            var PCCount = (from row in _context.ProductCategory
                                select row).Count();
            //Order Count
            var TotalOrder = (from row in _context.OrderDetails
                              select row).Count();
            //Customer Count
            var Customer = _userManager.Users.Count();

            ViewBag.PCount = ProductCount;
            ViewBag.PCCount = PCCount;
            ViewBag.TotalOrder = TotalOrder;
            ViewBag.Customer = Customer;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
