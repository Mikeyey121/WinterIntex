using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;

namespace WinterIntex.Components
{
    public class ProductTypeViewComponent : ViewComponent
    {

        private IProductRepository _waterRepo;
        // Constructor
        public ProductTypeViewComponent(IProductRepository temp)
        {
            _waterRepo = temp;
        }
        public IViewComponentResult Invoke()
        {

            ViewBag.SelectedProjectType = RouteData?.Values["projectType"];
            var projectTypes = _waterRepo.Products
                .Select(x => x.Year)
                .Distinct()
                .OrderBy(x => x);
            return View(projectTypes);
        }
    }
}
