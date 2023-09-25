using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<Users>> Get()
        {
            return _db.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Users> Get(int id)
        {
            Users user = _db.Users.Find(id);

            if (user == null) 
                return NotFound();

            return new ObjectResult(user);
        }

        [HttpPost]
        public ActionResult<Users> Post(Users user)
        {
            if (user == null)
                return BadRequest();

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(user);
        }

        [HttpPut]
        public ActionResult<Users> Put(Users user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!_db.Users.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            _db.Update(user);
            _db.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult<Users> Delete(int id)
        {
            Users user = _db.Users.Find(id);

            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            _db.SaveChanges();

            return Ok(user);
        }
    }
}
