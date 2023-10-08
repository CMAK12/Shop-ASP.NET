using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShopDbContext _db;

        public UsersController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Get()
        {
            return await _db.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Get(int id)
        {
            Users user = await _db.Users.FindAsync(id);

            if (user == null) 
                return NotFound();

            return new ObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> Post(Users user)
        {
            if (user == null)
                return BadRequest();

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<Users>> Put(Users user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!await _db.Users.AnyAsync(x => x.Id == user.Id))
            {
                return NotFound();
            }

            _db.Update(user);
            await _db.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> Delete(int id)
        {
            Users user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return Ok(user);
        }
    }
}
