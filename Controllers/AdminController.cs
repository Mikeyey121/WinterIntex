﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WinterIntex.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WinterIntex.Models.ViewModels;
using System.Drawing;

// This is the controller for all of the admin views

namespace WinterIntex.Controllers
{
    // We require that the user be logged in as an admin in order to access these actions and views
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // Creating global variables for the repository pattern, logger, and identity user manager
        private IProductRepository _repo;
        private IOrderRepository _oRepo;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        // Variable for enumerable of type product
        public IEnumerable<Product> CoolProduct { get; set; }

        // Constructor to instantiate global variables
        public AdminController(IProductRepository temp, ILogger<AdminController> logger, IOrderRepository oRepo, UserManager<IdentityUser> userManager)
        {
            _repo = temp;
            _logger = logger;
            _oRepo = oRepo;
            _userManager = userManager;
        }

        // Get request to display the products crud view
        public IActionResult Products()
        {
            // Query products including related categories
            var CoolProduct = _repo.Products
                .Include(p => p.PrimaryColor) 
                .Include(p => p.SecondaryColor)
                .ToList();

            return View(CoolProduct);
        }

        // Get request to display the fraudulent orders for the admin to review
        public IActionResult Orders()
        {
            var FraudOrders = _oRepo.Order.Where(x => x.Fraud == true)
                .Include(x => x.Customers)
                .OrderByDescending(x => x.Date)
            .ToList();

                return View(FraudOrders);
        }

        // Get request for the product form 
        [HttpGet]
        public IActionResult ProductForm()
        {
            ViewBag.Colors = _repo.Color
                .OrderBy(x => x.Color_Description)
                .ToList();
            return View(new Product());
        }

        // Post request for the product form 
        [HttpPost]
        public IActionResult ProductForm(Product response)
        {
            if (ModelState.IsValid)
            {
                _repo.CreateProduct(response);

                var CoolProduct = _repo.Products
                .Include(p => p.PrimaryColor)
                .Include(p => p.SecondaryColor)
                .ToList();

                return View("Products",CoolProduct);
            }
            else
            {
                ViewBag.Colors = _repo.Color
                .OrderBy(x => x.Color_Description)
                .ToList();
                return View("ProductCreate",response);
            }

        }

        // Get request for the edit view which is used to edit products
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var record = _repo.Products
                .Single(x => x.Product_ID == id);

           

            ViewBag.Colors = _repo.Color
                .OrderBy(x => x.Color_Description)
                .ToList();
            return View("ProductForm", record);
        }

        // Post request to post our product edits to the databse
        [HttpPost]
        public IActionResult Edit(Product products)
        {
            _repo.SaveProduct(products);
            return RedirectToAction("Products");
        }

        // Get request for the delete product confirmation screen 
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var record = _repo.Products
                .Single(x => x.Product_ID == id);

            return View("ProductDelete",record);
        }

        // Post request to actually delete a product
        [HttpPost]
        public IActionResult Delete(Product delete)
        {
            _repo.DeleteProduct(delete);
            

            return RedirectToAction("Products");
        }

        // Get request for the form to create a product
        [HttpGet]
        public IActionResult ProductCreate()
        {
            string maxProduct = _repo.Products.Max(x => x.Product_ID);
            maxProduct = $"{maxProduct}1";
            

            ViewBag.Colors = _repo.Color
                .OrderBy(x => x.Color_Description)
                .ToList();
            ViewBag.MaxProduct = maxProduct;
            return View();
        }

        // Get method for viewing the users to perform crud functionality 
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // Get method for editing a user
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return View("Error"); // Handle the case where the user is not found

            // Create and pass a view model if necessary
            return View(user);
        }

        // Post method for editing a user
        [HttpPost]
        public async Task<IActionResult> EditUser(IdentityUser user)
        {
            var userInDb = await _userManager.FindByIdAsync(user.Id);
            if (userInDb == null)
                return View("Error"); // Handle not found

            // Update properties and save changes
            userInDb.Email = user.Email;
            userInDb.UserName = user.UserName;
            userInDb.PhoneNumber = user.PhoneNumber;
            userInDb.TwoFactorEnabled = user.TwoFactorEnabled;
            userInDb.LockoutEnabled = user.LockoutEnabled;
            userInDb.LockoutEnd = user.LockoutEnd;
            userInDb.AccessFailedCount = user.AccessFailedCount;
            // Add other properties you want to update
            await _userManager.UpdateAsync(userInDb);

            var users = _userManager.Users.ToList();

            return RedirectToAction("ListUsers",users);
        }

        // Get method to confirm deleting a user 
        [HttpGet]
        public async Task<IActionResult> UserDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error"); // Or handle accordingly
            }

            var model = new UserDeleteViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(model);
        }

        // Post method for deleting a user in the database. 
        [HttpPost]
        public async Task<IActionResult> UserDeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return View("Error"); // Handle not found

            await _userManager.DeleteAsync(user);
            return RedirectToAction("ListUsers");
        }

    }
}
