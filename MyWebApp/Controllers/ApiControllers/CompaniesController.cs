using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public CompaniesController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Companies>>> Get()
        {
            return await _db.Companies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Companies>> Get(int id)
        {
            Companies company = await _db.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            return new ObjectResult(company);
        }

        [HttpPost]
        public async Task<ActionResult<Companies>> Post(Companies company)
        {
            if (company == null)
                return BadRequest();

            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();

            return Ok(company);
        }

        [HttpPut]
        public async Task<ActionResult<Companies>> Put(Companies company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            if (!await _db.Companies.AnyAsync(x => x.Id == company.Id))
            {
                return NotFound();
            }

            _db.Update(company);
            await _db.SaveChangesAsync();

            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Companies>> Delete(int id)
        {
            Companies company = await _db.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();

            return Ok(company);
        }
    }
}
