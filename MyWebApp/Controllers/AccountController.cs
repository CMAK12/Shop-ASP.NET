using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Helpers;
using MyWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopDbContext _db;

        public AccountController (ShopDbContext db, IConfiguration configuration)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Users user)
        {
            if (ModelState.IsValid)
            {
                bool isExistingUser = _db.Users.Any(person => 
                    person.Username == user.Username || person.Email == user.Email);

                if (isExistingUser)
                {
                    ModelState.AddModelError("Username", "Username or email is already taken");

                    return View(user);
                }

                user.Password = PasswordHelper.HashPassword(user.Password);
                user.Status = "user";
               
                _db.Users.Add(user);
                _db.SaveChanges();

                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Status)
                    };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            if (ModelState.IsValid)
            {
                Users? foundedUser = _db.Users.SingleOrDefault(person => 
                    person.Username == username || person.Email == username);
                bool verifyPassword = PasswordHelper.VerifyPassword(password, foundedUser?.Password);
                    
                if (foundedUser != null && verifyPassword)
                {
                    var claims = new List<Claim>() 
                    { 
                        new Claim(ClaimTypes.Name, foundedUser.Username),
                        new Claim(ClaimTypes.Role, foundedUser.Status) 
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Username", "Username or password incorrect");

                return View();
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
