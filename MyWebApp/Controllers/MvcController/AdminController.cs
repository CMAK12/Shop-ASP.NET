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
        public async Task<IActionResult> AddNewProductAsync(Products product, IFormFile photo)
        {
            if (ModelState.IsValid && photo != null)
            {
                product.Photo = await UploadPhotoHelper.UploadPhotoAsync(photo, _appEnvironment);

                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();

                return RedirectToAction("Products");
            }

            return View();
        }

        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            Products? productToDelete = await _db.Products.FindAsync(productId);

            if (productToDelete == null)
                return NotFound();

            DeletePhotoHelper.DelPhoto(productToDelete.Photo, _appEnvironment);

            _db.Products.Remove(productToDelete);
            await _db.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        [HttpGet]
        public async Task<IActionResult> EditProductAsync(int productId)
        {
            var currentProduct = await _db.Products.FindAsync(productId);
            ViewBag.Companies = _db.Companies;

            if (currentProduct == null)
                return NotFound();

            return View(currentProduct);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductAsync(Products product, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                Products? productToEdit = await _db.Products.FindAsync(product.Id);

                if (photo != null)
                    product.Photo = await UpdatePhotoHepler.UpdatePhotoAsync(product.Photo, photo, _appEnvironment);
                else
                    product.Photo = productToEdit.Photo;

                _db.Entry(productToEdit).CurrentValues.SetValues(product);
                await _db.SaveChangesAsync();

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
        public async Task<IActionResult> AddNewCompanyAsync(Companies company)
        {
            if (ModelState.IsValid)
            {
                await _db.Companies.AddAsync(company);
                await _db.SaveChangesAsync();

                return RedirectToAction("Companies");
            }

            return View();
        }

        public async Task<IActionResult> DeleteCompanyAsync(int companyId)
        {
            var companyToDelete = await _db.Companies.FindAsync(companyId);

            if (companyToDelete == null)
                return NotFound();

            _db.Companies.Remove(companyToDelete);
            await _db.SaveChangesAsync();

            return RedirectToAction("Companies");
        }

        [HttpGet]
        public async Task<IActionResult> EditCompanyAsync(int companyId)
        {
            var currentCompany = await _db.Companies.FindAsync(companyId);

            if (currentCompany == null)
                return NotFound();

            return View(currentCompany);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyAsync(Companies company)
        {
            if (ModelState.IsValid)
            {
                Companies companyToEdit = await _db.Companies.FindAsync(company.Id);

                companyToEdit.Name = company.Name;

                await _db.SaveChangesAsync();

                return RedirectToAction("Companies");
            }

            return View();
        }

        public IActionResult Users()
        {
            return View(_db.Users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserAsync(int userId)
        {
            var currentUser = await _db.Users.FindAsync(userId);

            if (currentUser == null)
                return NotFound();

            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserAsync(int userId, string username, string email, string status)
        {
            if (ModelState.IsValid)
            {
                var userToEdit = await _db.Users.FindAsync(userId);

                userToEdit.Username = username;
                userToEdit.Email = email;
                userToEdit.Status = status;

                await _db.SaveChangesAsync();

                return RedirectToAction("Users");
            }

            return View();
        }

        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            var userToDelete = await _db.Companies.FindAsync(userId);

            if (userToDelete == null)
                return NotFound();

            _db.Companies.Remove(userToDelete);
            await _db.SaveChangesAsync();

            return RedirectToAction("Users");
        }
    }
}
