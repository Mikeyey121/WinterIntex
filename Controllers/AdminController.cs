using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WinterIntex.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WinterIntex.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private WinterIntexContext _context;
        public IEnumerable<Product> Product { get; set; }

        public AdminController(WinterIntexContext temp)
        {
            _context = temp;
        }
        public IActionResult Products()
        {
            var selectedCategory = RouteData?.Values["categoryDescription"];

            ViewBag.SelectedCategory = selectedCategory;

            // Query products including related categories
            var products = _context.Products
                .Include(p => p.CategoryProductOrders)
                    .ThenInclude(cpo => cpo.Category)
                .OrderBy(p => p.Name)
                .ToList();

            return View(products);
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
    }
}
