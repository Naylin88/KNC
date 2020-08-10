using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InitCMS.Data;
using InitCMS.Models;
using InitCMS.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace InitCMS.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly InitCMSContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public ProductsController(InitCMSContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
          
        }
        
        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
               return RedirectToAction("Login","Admin");
  
            }

            var initCMSContext = _context.Products.Include(p => p.ProductCategory)
                .Include(c=>c.Category);
           
            return View(await initCMSContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
                return RedirectToAction("Login", "Admin");

            }

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p=> p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        
        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionEmail") == null)
            {

                return RedirectToAction("Login", "Admin");
            }
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "Id", "Name");
            ViewData["CategoryCatId"] = new SelectList(_context.Category, "CatId", "CatTitle");
            return View();
        }

        //Remote Validation for Product Code
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPCode(string pcode)
        {

            var query = await _context.Products.FirstOrDefaultAsync(c => c.PCode == pcode);
          
            if (query == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Product Code {pcode} is already in use. Please Choose Different Code!");
            }

        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PCode,Description,PurchasePrice,SellPrice,InStock,Sale,CreatedDate,ProductCategoryID,CategoryCatId,Photo")] ProductViewModel product)
        {
            
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(product);

                Product products = new Product
                {
                    Name = product.Name,
                    PCode = product.PCode,
                    Description = product.Description,
                    PurchasePrice = (decimal)product.PurchasePrice,
                    SellPrice = product.SellPrice,
                    InStock = product.InStock,
                    Sale = product.Sale,
                    CreatedDate = DateTime.Now,
                    ProductCategoryID = product.ProductCategoryID,
                    CategoryCatId = product.CategoryCatId,
                    ImagePath = uniqueFileName,
                };
               // product = await _context.Products.SingleOrDefaultAsync(c => c.PCode == products.PCode);
              
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryID);
            ViewData["CategoryCatId"] = new SelectList(_context.Category, "CatId", "CatTitle", product.CategoryCatId);
            return View(product);
        }

        private string UploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Photo.CopyTo(fileStream);
                fileStream.Dispose();
            }
            return uniqueFileName;
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionEmail")))
            {
                return RedirectToAction("Login", "Admin");

            }

            if (HttpContext.Session.GetString("SessionEmail") == null)
            {

                return RedirectToAction("Login", "Admin");
            }

            if (id == null)
            {
                return NotFound();
            }

            var product  = await _context.Products.FindAsync(id);
           
            ProductEditViewModel PEVM = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                PCode = product.PCode,
                Description = product.Description,
                PurchasePrice = product.PurchasePrice,
                SellPrice = (decimal)product.SellPrice,
                InStock = product.InStock,
                Sale = product.Sale,
                CreatedDate = product.CreatedDate,
                ProductCategoryID = product.ProductCategoryID,
                CategoryCatId = product.CategoryCatId,
                PhtotPath = product.ImagePath
                
            };
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryID);
            ViewData["CategoryCatId"] = new SelectList(_context.Category, "CatId", "CatTitle", product.CategoryCatId);
            return View(PEVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,PCode,Description,PurchasePrice,SellPrice,InStock,Sale,CreatedDate,ProductCategoryID,CategoryCatId,Photo,PhtotPath")] ProductEditViewModel productVM)
        {
            if (id != productVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string uniqueFileName = UploadedFile(productVM);

                        Product products = new Product
                        {
                            Id = productVM.Id,
                            Name = productVM.Name,
                            PCode = productVM.PCode,
                            Description = productVM.Description,
                            PurchasePrice = (decimal)productVM.PurchasePrice,
                            SellPrice = productVM.SellPrice,
                            InStock = productVM.InStock,
                            Sale = productVM.Sale,
                            CreatedDate = DateTime.Now,
                            ProductCategoryID = productVM.ProductCategoryID,
                            CategoryCatId = productVM.CategoryCatId
                        };
                        
                        if (uniqueFileName == null)
                        {
                            products.ImagePath = productVM.PhtotPath;
                        }
                        else if (productVM.PhtotPath != null)
                        {
                            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "images", productVM.PhtotPath);
                            System.IO.File.Delete(filePath);
                            products.ImagePath = uniqueFileName;
                        }
                        else
                        {
                            products.ImagePath = uniqueFileName;
                        }

                        _context.Update(products);
                        _context.SaveChanges();
                        
                      //  return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "Id", "Name", productVM.ProductCategoryID);
            ViewData["CategoryCatId"] = new SelectList(_context.Category, "CatId", "CatTitle", productVM.CategoryCatId);
            return View(productVM);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (HttpContext.Session.GetString("SessionEmail") == null)
            {

                return RedirectToAction("Login", "Admin");
            }

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
