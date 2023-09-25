using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<Products>> Get()
        {
            return _db.Products.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Products> Get(int id)
        {
            Products product = _db.Products.Find(id);

            if (product == null) 
                return NotFound();

            return product;
        }

        [HttpPost]
        public ActionResult<Products> Post(Products product)
        {
            if (product == null)
                return BadRequest();

            _db.Products.Add(product);
            _db.SaveChanges();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public ActionResult<Products> Put(Products product)
        {
            if (product == null)
                return BadRequest();

            if (!_db.Products.Any(x => x.Id == product.Id))
                return NotFound();

            _db.Update(product);
            _db.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public ActionResult<Products> Delete(int id)
        {
            Products product = _db.Products.Find(id);

            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();

            return Ok(product);
        }
    }
}
