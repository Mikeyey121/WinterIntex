using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(int pageNum = 1, string? year = null)
        {
            int pageSize = 10;

            var blah = new ProductsListViewModel
            {
                Products = _repo.Products
                .Where(x => x.Year.ToString() == year || year == null)
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = year == null ? _repo.Products.Count() : _repo.Products.Where(x => x.Year.ToString() == year).Count()

                },
                CurrentProjectType = year

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
