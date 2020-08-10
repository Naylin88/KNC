using InitCMS.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace InitCMS.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly InitCMSContext _context;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(InitCMSContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }
        public void CreateOrder(Order order)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    order.OrderPlaced = DateTime.Now;
                    _context.Orders.AddRange(order);
                    _context.SaveChanges();
                    var shoppingCartItems = _shoppingCart.ShoppingCartItems;

                    foreach (var item in shoppingCartItems)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id,
                            OrderId = order.OrderId,
                            Price = (decimal)item.Product.SellPrice
                        };
                        _context.OrderDetails.AddRange(orderDetail);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
