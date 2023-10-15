using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using System.Diagnostics;

namespace MyWebApp.Controllers.MvcController
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

        public IActionResult Index(string? companyName)
        {
            ViewBag.Companies = _db.Companies;
            ViewBag.SelectedCompany = companyName;

            if (companyName != null)
            {
                var filteredList = _db.Products.Where(product => product.Company == companyName);

                return View(filteredList);
            }

            return View(_db.Products);
        }

        public async Task<IActionResult> ProductDetailAsync(int productId)
        {
            var foundedProduct = await _db.Products.FindAsync(productId);

            if (foundedProduct == null)
                return NotFound();

            return View(foundedProduct);
        }

        public string Nothing()
        {
            return "Ne chinazes";
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}