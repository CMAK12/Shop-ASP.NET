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

        public async Task AddToCartAsync(Products product)
        {
            Id = GetCartId();

            var item = await _db.CartItem.SingleOrDefaultAsync(c => c.CartId == Id && c.Product.Id == product.Id);

            if (item == null)
            {
                item = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CartId = Id,
                    Product = product,
                    Quantity = 1,
                };

                await _db.CartItem.AddAsync(item);
            }
            else
                item.Quantity += 1;

            await _db.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            if (cartItem != null)
            {
                _db.CartItem.Remove(cartItem);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            Id = GetCartId();

            return await _db.CartItem.Include(c => c.Product).Where(c => c.CartId == Id).ToListAsync();
        }
    }
}
