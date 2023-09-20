using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers
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

        public IActionResult Index()
        {
            var items = _cart.GetCartItems();

            return View(items);
        }

        
        public RedirectToActionResult AddToCart(int id)
        {
            var item = _db.Products.FirstOrDefault(i => i.Id == id);

            if (item != null)
                _cart.AddToCart(item);

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveItem(int id, string cartId) 
        {
            var item = _db.CartItem.SingleOrDefault(item => item.Product.Id == id && item.CartId == cartId);

            if (item != null)
                _cart.RemoveFromCart(item);

            return RedirectToAction("Index");
        }
    }
}
