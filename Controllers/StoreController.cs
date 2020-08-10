using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InitCMS.Data;
using InitCMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace InitCMS.Controllers
{
    public class StoreController : Controller
    {
        private readonly InitCMSContext _context;

        public StoreController(InitCMSContext context)
        {
            _context = context;
        }
        // GET: Products
        public IActionResult Index()
        {
            dynamic dy = new ExpandoObject();
            dy.products = GetProducts();
            dy.productCategories = GetProductCategories();
            return View(dy);
        }

        public List<Product> GetProducts() 
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }
        public List<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> productCategories = _context.ProductCategory.ToList();
            return productCategories;
        }
        //Get: List filter by Category
        public async Task<IActionResult> ProductByCategory(int? Id)
        {
            
            var initCMSContext = _context.Products.Include(c => c.ProductCategory).Where(x => x.ProductCategoryID == Id);

            return View(await initCMSContext.ToListAsync());
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


    }
}
