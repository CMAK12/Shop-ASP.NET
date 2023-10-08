using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Controllers.MvcController
{
    public class CartController : Controller
    {
        private readonly Cart _cart;
        private readonly ShopDbContext _db;

        public CartController(Cart cart, ShopDbContext db)
        {
            _cart = cart;
            _db = db;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var items = await _cart.GetCartItemsAsync();
            ViewBag.TotalPrice = 0;

            foreach (var item in items)
            {
                ViewBag.TotalPrice += item.Product.Price * item.Quantity;
            }

            return View(items);
        }

        public async Task<RedirectToActionResult> AddToCartAsync(int id)
        {
            var item = await _db.Products.FirstOrDefaultAsync(i => i.Id == id);

            if (item != null)
                _cart.AddToCartAsync(item);

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> RemoveItemAsync(int id, string cartId)
        {
            var item = await _db.CartItem.SingleOrDefaultAsync(item => item.Product.Id == id && item.CartId == cartId);

            if (item != null)
                _cart.RemoveFromCartAsync(item);

            return RedirectToAction("Index");
        }
    }
}
