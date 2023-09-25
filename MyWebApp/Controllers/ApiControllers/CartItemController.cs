using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public CartItemController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CartItem>> Get()
        {
            return _db.CartItem.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<CartItem> Get(int id)
        {
            CartItem cartItem = _db.CartItem.Find(id);

            if (cartItem == null)
                return NotFound();

            return new ObjectResult(cartItem);
        }

        [HttpPost]
        public ActionResult<CartItem> Post(CartItem cartItem)
        {
            if (cartItem == null)
                return BadRequest();

            _db.CartItem.Add(cartItem);
            _db.SaveChanges();

            return Ok(cartItem);
        }

        [HttpPut]
        public ActionResult<CartItem> Put(CartItem cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest();
            }
            if (!_db.CartItem.Any(x => x.ItemId == cartItem.ItemId))
            {
                return NotFound();
            }

            _db.Update(cartItem);
            _db.SaveChanges();

            return Ok(cartItem);
        }

        [HttpDelete("{id}")]
        public ActionResult<CartItem> Delete(int id)
        {
            CartItem cartItem = _db.CartItem.Find(id);

            if (cartItem == null)
                return NotFound();

            _db.CartItem.Remove(cartItem);
            _db.SaveChanges();

            return Ok(cartItem);
        }
    }
}
