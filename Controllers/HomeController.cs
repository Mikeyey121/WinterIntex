using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WinterIntex.Models;
using WinterIntex.Models.ViewModels;


namespace WinterIntex.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository _repo;

        public HomeController(IProductRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index(string? categoryDescription = null,int pageNum = 1, string? color = null)
        {
            int pageSize = 10;

            IQueryable<Product> productsQuery = _repo.Products
            .Include(p => p.CategoryProductOrders) // Include the join table
            .ThenInclude(cpo => cpo.Category); // Include the category


            // Filter by year if specified
            if (!string.IsNullOrEmpty(color))
            {
                productsQuery = productsQuery.Where(x => x.Year.ToString() == color);
            }

            // Filter by category description if specified
            if (!string.IsNullOrEmpty(categoryDescription))
            {
                productsQuery = productsQuery.Where(x => x.CategoryProductOrders.Any(cpo => cpo.Category.Category_Description == categoryDescription));
            }

            var blah = new ProductsListViewModel
            {
                Products = productsQuery
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = categoryDescription == null ? _repo.Products.Count() : _repo.Products.Where(x => x.CategoryProductOrders.Any(cpo => cpo.Category.Category_Description == categoryDescription)).Count()

                },
                CurrentCategoryDescription = categoryDescription,
                CurrentProjectColor = color

            };

            return View(blah);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
