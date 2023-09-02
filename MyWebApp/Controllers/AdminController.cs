using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyWebApp.Models;
using MyWebApp.Helpers;

namespace MyWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ShopDbContext _db;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminController(ShopDbContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View(_db.Products);
        }

        public IActionResult ProductDetail(int productId)
        {
            return View(_db.Products.Where(product => product.Id == productId));
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            ViewBag.Companies = _db.Companies;

            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(Products product, IFormFile photo)
        {
            if (ModelState.IsValid && photo != null) 
            {
                product.Photo = UploadPhotoHelper.UploadPhoto(photo, _appEnvironment);

                _db.Products.Add(product);
                _db.SaveChanges();

                return RedirectToAction("Products");
            }

            return View();
        }

        public IActionResult DeleteProduct(int productId)
        {
            Products? productToDelete = _db.Products.Find(productId);

            _db.Products.Remove(productToDelete);
            _db.SaveChanges();

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult EditProduct(int productId)
        {
            ViewBag.Companies = _db.Companies;

            return View(_db.Products.Find(productId));
        }

        [HttpPost]
        public IActionResult EditProduct(Products product, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                Products? productToEdit = _db.Products.Find(product.Id);

                if (photo != null)
                    product.Photo = UpdatePhotoHepler.UpdatePhoto(product.Photo, photo, _appEnvironment);
                else 
                    product.Photo = productToEdit.Photo;

                _db.Entry(productToEdit).CurrentValues.SetValues(product);
                _db.SaveChanges();

                return RedirectToAction("Products");
            }

            return View();
        }

        public IActionResult Companies()
        {
            return View(_db.Companies);
        }

        [HttpGet]
        public IActionResult AddNewCompany()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewCompany(Companies company)
        {
            if (ModelState.IsValid)
            {
                _db.Companies.Add(company);
                _db.SaveChanges();

                return RedirectToAction("Companies");
            }

            return View();
        }

        public IActionResult DeleteCompany(int companyId)
        {
            var companyToDelete = _db.Companies.Find(companyId);

            _db.Companies.Remove(companyToDelete);
            _db.SaveChanges();

            return RedirectToAction("Companies");
        }

        [HttpGet]
        public IActionResult EditCompany(int companyId)
        {
            return View(_db.Companies.Find(companyId));
        }

        [HttpPost]
        public IActionResult EditCompany(Companies company)
        {
            if (ModelState.IsValid)
            {
                Companies companyToEdit = _db.Companies.Find(company.Id);

                companyToEdit.Name = company.Name;

                _db.SaveChanges();

                return RedirectToAction("Companies");
            }

            return View();
        }

        public IActionResult Users()
        {
            return View(_db.Users);
        }

        public IActionResult UserDetail()
        {
            return View();
        }
    }
}
