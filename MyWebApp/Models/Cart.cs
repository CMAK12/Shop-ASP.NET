using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MyWebApp.Models
{
    public class Cart
    {
        private readonly ShopDbContext _db;
        private const string CartSessionKey = "CartId";
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClaimsIdentity identity = new ClaimsIdentity();

        public Cart(ShopDbContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        public string Id { get; set; }

        public string GetCartId()
        {
            ISession session = _contextAccessor.HttpContext.Session;

            if (session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(identity.Name))
                {
                    session.SetString(CartSessionKey, identity.Name);
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }

            return session.GetString(CartSessionKey);
        }

        public void AddToCart(Products product)
        {
            Id = GetCartId();

            var item = _db.CartItem.SingleOrDefault(c => c.CartId == Id && c.Product.Id == product.Id);

            if (item == null)
            {
                item = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CartId = Id,
                    Product = product,
                    Quantity = 1,
                };

                _db.CartItem.Add(item);
            }
            else
            {
                item.Quantity += 1;
            }

            _db.SaveChanges();
        }

        public void RemoveFromCart(CartItem cartItem)
        {
            if (cartItem != null)
            {
                _db.CartItem.Remove(cartItem);
                _db.SaveChanges();
            }
        }

        public List<CartItem> GetCartItems()
        {
            Id = GetCartId();

            return _db.CartItem.Include(c => c.Product).Where(c => c.CartId == Id).ToList();
        }
    }
}
