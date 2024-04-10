using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using WinterIntex.Models;

namespace WinterIntex.Controllers
{
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
            Product = _context.Products
                .OrderBy(x => x.Name).ToList();
            return View(Product);
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
