using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<Companies>> Get()
        {
            return _db.Companies.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Companies> Get(int id)
        {
            Companies company = _db.Companies.Find(id);

            if (company == null)
                return NotFound();

            return new ObjectResult(company);
        }

        [HttpPost]
        public ActionResult<Companies> Post(Companies company)
        {
            if (company == null)
                return BadRequest();

            _db.Companies.Add(company);
            _db.SaveChanges();

            return Ok(company);
        }

        [HttpPut]
        public ActionResult<Companies> Put(Companies company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            if (!_db.Companies.Any(x => x.Id == company.Id))
            {
                return NotFound();
            }

            _db.Update(company);
            _db.SaveChanges();

            return Ok(company);
        }

        [HttpDelete("{id}")]
        public ActionResult<Companies> Delete(int id)
        {
            Companies company = _db.Companies.Find(id);

            if (company == null)
                return NotFound();

            _db.Companies.Remove(company);
            _db.SaveChanges();

            return Ok(company);
        }
    }
}
