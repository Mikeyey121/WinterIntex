using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;
using WinterIntex.Models;
using WinterIntex.Models.ViewModels;

// This is the home controller for most of the base functionality 
namespace WinterIntex.Controllers
{
    public class HomeController : Controller
    {

        // Creating global variables for the repository pattern, products, and recommendations
        private IProductRepository _repo;
        public Product UserRec1 { get; set; } = new Product();
        public Product UserRec2 { get; set; } = new Product();
        public Product UserRec3 { get; set; } = new Product();
        
        public string customer_ID { get; set; }
        public UserRecommendations UserRecommendations { get; set; } = new UserRecommendations();

        // Constructor to instantiate the repo
        public HomeController(IProductRepository temp)
        {
            _repo = temp;
        }
        
        // Get request to display the products page with all of the filters
        public IActionResult Products(int pageSizes = 10, int pageNum = 1, string? color = null, string? categoryDescription = null)
        {
            int pageSize = pageSizes;

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

            var productsLists = new ProductsListViewModel
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

            return View(productsLists);
        }

        // Get request for our privacy view
        public IActionResult Privacy()
        {
            return View();
        }

        // Get request for our index page with functionality based on who is logged in
        public IActionResult Index()
        {
            var viewModel = new UserRecommendationsViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };


            if (viewModel.IsAuthenticated)
            {
                // Mhutch customer id 218756d3-239e-46e6-a9f7-8b0c4c72fc2b is on row 13198
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                viewModel.UserRecommendations = _repo.UserRecommendations.FirstOrDefault(x => x.customer_ID == userId);

                var recommendation1Id = viewModel.UserRecommendations.Recommendation_1;
                var product1 = _repo.Products.FirstOrDefault(x => x.Product_ID == recommendation1Id);


                viewModel.RecommendedProducts.Add(product1);
                viewModel.RecommendedProducts.Add(_repo.Products.FirstOrDefault(x => x.Product_ID == viewModel.UserRecommendations.Recommendation_2));
                viewModel.RecommendedProducts.Add(_repo.Products.FirstOrDefault(x => x.Product_ID == viewModel.UserRecommendations.Recommendation_3));
                viewModel.RecommendedProducts.Add(_repo.Products.FirstOrDefault(x => x.Product_ID == viewModel.UserRecommendations.Recommendation_4));
                viewModel.RecommendedProducts.Add(_repo.Products.FirstOrDefault(x => x.Product_ID == viewModel.UserRecommendations.Recommendation_5));


            }
            else
            {


                foreach (var r in _repo.TopRecommendations)
                {
                    viewModel.TopRecommendations.Add(new TopRecommendations { Product_ID = r.Product_ID, Average_Rating = r.Average_Rating });
                    viewModel.RecommendedProducts.Add(_repo.Products.FirstOrDefault(x => x.Product_ID == r.Product_ID));

                }

            }
            return View(viewModel);
        }

        // Get request for the about page
        public IActionResult About()
        {
            return View();
        }

        // Get request for the navigation to the admin pages and authorizing who can access it
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        // Error view
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
