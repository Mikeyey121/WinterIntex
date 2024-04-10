using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for .Include() and .ThenInclude()
using WinterIntex.Models;
using System.Linq;

namespace WinterIntex.Components
{
    public class ProductTypeViewComponent : ViewComponent
    {
        private IProductRepository _repo;

        public ProductTypeViewComponent(IProductRepository repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke()
        {
            var selectedCategory = RouteData?.Values["categoryDescription"];

            ViewBag.SelectedCategory = selectedCategory;
            // Query to include categories through the linking table
            var categories = _repo.Products
                .Include(p => p.CategoryProductOrders) // Include the linking table
                .ThenInclude(cpo => cpo.Category) // Then include the Category
                .SelectMany(p => p.CategoryProductOrders.Select(cpo => cpo.Category.Category_Description)) // Select the Category Descriptions
                .Distinct()
                .OrderBy(x => x);

            return View(categories);
        }
    }
}