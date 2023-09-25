using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyWebApp.Models;
using MyWebApp.Helpers;

namespace MyWebApp.Controllers.MvcController
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View(_db.Products);
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

            if (productToDelete == null)
                return NotFound();

            DeletePhotoHelper.DelPhoto(productToDelete.Photo, _appEnvironment);

            _db.Products.Remove(productToDelete);
            _db.SaveChanges();

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult EditProduct(int productId)
        {
            var currentProduct = _db.Products.Find(productId);
            ViewBag.Companies = _db.Companies;

            if (currentProduct == null)
                return NotFound();

            return View(currentProduct);
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

            if (companyToDelete == null)
                return NotFound();

            _db.Companies.Remove(companyToDelete);
            _db.SaveChanges();

            return RedirectToAction("Companies");
        }

        [HttpGet]
        public IActionResult EditCompany(int companyId)
        {
            var currentCompany = _db.Companies.Find(companyId);

            if (currentCompany == null)
                return NotFound();

            return View(currentCompany);
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
            return View(_db.Companies);
        }

        [HttpGet]
        public IActionResult EditUser(int userId)
        {
            var currentUser = _db.Companies.Find(userId);

            if (currentUser == null)
                return NotFound();

            return View(currentUser);
        }

        [HttpPost]
        public IActionResult EditUser(int userId, string username, string email, string status)
        {
            if (ModelState.IsValid)
            {
                var userToEdit = _db.Users.Find(userId);

                userToEdit.Username = username;
                userToEdit.Email = email;
                userToEdit.Status = status;

                _db.SaveChanges();

                return RedirectToAction("Users");
            }

            return View();
        }

        public IActionResult DeleteUser(int userId)
        {
            var userToDelete = _db.Companies.Find(userId);

            if (userToDelete == null)
                return NotFound();

            _db.Companies.Remove(userToDelete);
            _db.SaveChanges();

            return RedirectToAction("Users");
        }
    }
}
