using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using System.Diagnostics;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopDbContext _db;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(ShopDbContext options, IWebHostEnvironment appEnvironment)
        {
            _db = options;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.Companies = _db.Companies;

            return View(_db.Products);
        }

        public IActionResult ProductDetail(int id)
        {
            return View(_db.Products.Find(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}