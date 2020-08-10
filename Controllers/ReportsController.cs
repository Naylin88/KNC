using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InitCMS.ViewModel;
using InitCMS.Data;
using System;
using Microsoft.AspNetCore.Http;

namespace InitCMS.Controllers
{
    public class ReportsController : Controller
    {
        private readonly InitCMSContext _context;
        public ReportsController(InitCMSContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
                return RedirectToAction("Login", "Admin");

            }

            return View();
        }

        public IActionResult DailySale(DateTime date)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
                return RedirectToAction("Login", "Admin");

            }
            DateTime startDateTime = date; //Today at 00:00:00
            DateTime endDateTime = date.AddDays(1).AddTicks(-1); //Today at 23:59:59

            List<DailySaleViewModel> dsvm = new List<DailySaleViewModel>();
            var q = (from o in _context.Orders
                         join od in _context.OrderDetails on o.OrderId equals od.OrderId
                         join p in _context.Products on od.ProductId equals p.Id
                         where ( o.OrderPlaced >= startDateTime && o.OrderPlaced <= endDateTime )
                         select new
                         {
                             p.PCode,
                             p.Name,
                             o.FirstName,
                             od.Quantity,
                             od.Price,
                             o.OrderPlaced
                         }).ToList();

            foreach (var item in q)
            {
                DailySaleViewModel DsaleVM = new DailySaleViewModel();
                DsaleVM.CustomerName = item.FirstName;
                DsaleVM.PCode = item.PCode;
                DsaleVM.ProductName = item.Name;
                DsaleVM.Qty = item.Quantity;
                DsaleVM.Price = item.Price;
                DsaleVM.Total = item.Quantity * item.Price;
                DsaleVM.Date = item.OrderPlaced;
                dsvm.Add(DsaleVM);
            }
            
            return View(dsvm);
        }
        public IActionResult MonthlySale(DateTime date)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
                return RedirectToAction("Login", "Admin");

            }
            var month = Convert.ToDateTime(date).Month;
            var year = Convert.ToDateTime(date).Year;

            DateTime dt = date;
            int months = Convert.ToInt32(dt.Month);

            List<MonthlySaleViewModel> msvm = new List<MonthlySaleViewModel>();
            var q = (from o in _context.Orders
                     join od in _context.OrderDetails on o.OrderId equals od.OrderId
                     join p in _context.Products on od.ProductId equals p.Id
                     where (o.OrderPlaced.Month == month && o.OrderPlaced.Year == year)
                     select new
                     {
                         p.PCode,
                         p.Name,
                         o.FirstName,
                         od.Quantity,
                         od.Price,
                         o.OrderPlaced
                     }).ToList();

            foreach (var item in q)
            {
                MonthlySaleViewModel MsaleVM = new MonthlySaleViewModel();
                MsaleVM.CustomerName = item.FirstName;
                MsaleVM.PCode = item.PCode;
                MsaleVM.ProductName = item.Name;
                MsaleVM.Qty = item.Quantity;
                MsaleVM.Price = item.Price;
                MsaleVM.Total = item.Quantity * item.Price;
                MsaleVM.Date = item.OrderPlaced;
                msvm.Add(MsaleVM);
            }

            return View(msvm);
        }
    }
}
