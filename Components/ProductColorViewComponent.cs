using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;
using Microsoft.EntityFrameworkCore; // Required for .Include() and .ThenInclude()


namespace WinterIntex.Components
{
    public class ProductColorViewComponent :ViewComponent
    {
        private IProductRepository _repo;

        public ProductColorViewComponent(IProductRepository repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke()
        {

            var selectedColor = RouteData?.Values["color"];

            ViewBag.SelectedColor = selectedColor;
            // Query to include categories through the linking table
            var primaryColors = _repo.Products
                .Include(p => p.PrimaryColor) // Include the PrimaryColor navigation property
                .Select(p => p.PrimaryColor.Color_Description) // Select the Color_Description from the PrimaryColor
                .Distinct(); // Ensure only distinct primary color descriptions are selected

            var secondaryColors = _repo.Products
                .Include(p => p.SecondaryColor) // Include the SecondaryColor navigation property
                .Select(p => p.SecondaryColor.Color_Description) // Select the Color_Description from the SecondaryColor
                .Distinct(); // Ensure only distinct secondary color descriptions are selected

            var colors = primaryColors.Union(secondaryColors) // Combine primary and secondary colors, removing duplicates
                .OrderBy(c => c); // Order the color descriptions


            return View(colors);
        }
    }
}
