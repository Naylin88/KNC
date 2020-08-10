using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using InitCMS.Data;
using InitCMS.Models;
using InitCMS.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace InitCMS.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly InitCMSContext _context;
        public ShoppingCartController(InitCMSContext context, ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
            _context = context;
        }
        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var scvm = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(scvm);
        }

        public RedirectToActionResult AddToShoppingCart(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);
            }
            return RedirectToAction("Index","Store");

        }

        public RedirectToActionResult RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                _shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }


    }
}
