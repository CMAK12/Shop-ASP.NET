using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Helpers;
using MyWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Controllers.MvcController
{
    public class AccountController : Controller
    {
        private readonly ShopDbContext _db;

        public AccountController(ShopDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync(Users user)
        {
            if (ModelState.IsValid)
            {
                bool isExistingUser = await _db.Users.AnyAsync(person =>
                    person.Username == user.Username || person.Email == user.Email);

                if (isExistingUser)
                {
                    ModelState.AddModelError("Username", "Username or email is already taken");

                    return View(user);
                }

                user.Password = PasswordHelper.Initialize().HashPassword(user.Password);
                user.Status = "user";

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

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
        public async Task<IActionResult> LogInAsync(string username, string password)
        {
            if (ModelState.IsValid)
            {
                Users? foundedUser = await _db.Users.SingleOrDefaultAsync(person =>
                    person.Username == username || person.Email == username);
                bool verifyPassword = PasswordHelper.Initialize().VerifyPassword(password, foundedUser?.Password);

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
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
