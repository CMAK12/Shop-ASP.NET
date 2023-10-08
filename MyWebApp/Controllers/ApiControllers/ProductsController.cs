using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public ProductsController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            return await _db.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> Get(int id)
        {
            Products product = await _db.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Products>> Post(Products product)
        {
            if (product == null)
                return BadRequest();

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Products>> Put(Products product)
        {
            if (product == null)
                return BadRequest();

            if (!await _db.Products.AnyAsync(x => x.Id == product.Id))
                return NotFound();

            _db.Update(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Products>> Delete(int id)
        {
            Products product = await _db.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }
    }
}
