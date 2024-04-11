using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Products(string? categoryDescription = null,int pageNum = 1, string? color = null)
        {
            int pageSize = 10;

            IQueryable<Product> productsQuery = _repo.Products
            .Include(p => p.CategoryProductOrders) // Include the join table
            .ThenInclude(cpo => cpo.Category); // Include the category

            int TotalItem = _repo.Products.Count();

            // Filter by color if specified
            if (!string.IsNullOrEmpty(color))
            {
                productsQuery = productsQuery.Where(x =>
                    (x.PrimaryColor != null && x.PrimaryColor.Color_Description == color) ||
                    (x.SecondaryColor != null && x.SecondaryColor.Color_Description == color)
);
                TotalItem = color == null ? _repo.Products.Count() :
                        _repo.Products.Where(x =>
                            (x.PrimaryColor != null && x.PrimaryColor.Color_Description == color) ||
                            (x.SecondaryColor != null && x.SecondaryColor.Color_Description == color)
                        ).Count();


            }


            // Filter by category description if specified
            if (!string.IsNullOrEmpty(categoryDescription))
            {
                productsQuery = productsQuery.Where(x => x.CategoryProductOrders.Any(cpo => cpo.Category.Category_Description == categoryDescription));
                TotalItem = categoryDescription == null ? _repo.Products.Count() : _repo.Products.Where(x => x.CategoryProductOrders.Any(cpo => cpo.Category.Category_Description == categoryDescription)).Count();

            };



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
                    TotalItems = TotalItem
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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        //[Authorize]
        public IActionResult Admin()
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
