using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase, IRest<CartItem>
    {
        private readonly ShopDbContext _db;

        public CartItemController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> Get()
        {
            return await _db.CartItem.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> Get(int id)
        {
            CartItem cartItem = await _db.CartItem.FindAsync(id);

            if (cartItem == null)
                return NotFound();

            return new ObjectResult(cartItem);
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> Post(CartItem cartItem)
        {
            if (cartItem == null)
                return BadRequest();

            await _db.CartItem.AddAsync(cartItem);
            await _db.SaveChangesAsync();

            return Ok(cartItem);
        }

        [HttpPut]
        public async Task<ActionResult<CartItem>> Put(CartItem cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest();
            }
            if (!await _db.CartItem.AnyAsync(x => x.ItemId == cartItem.ItemId))
            {
                return NotFound();
            }

            _db.Update(cartItem);
            await _db.SaveChangesAsync();

            return Ok(cartItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CartItem>> Delete(int id)
        {
            CartItem cartItem = await _db.CartItem.FindAsync(id);

            if (cartItem == null)
                return NotFound();

            _db.CartItem.Remove(cartItem);
            await _db.SaveChangesAsync();

            return Ok(cartItem);
        }
    }
}