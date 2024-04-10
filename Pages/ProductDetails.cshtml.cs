using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WinterIntex.Models;

namespace WinterIntex.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly ILogger<ProductDetailsModel> _logger;
        // This is the private variable for the repository
        private IProductRepository _repo;

        // This is the variable for the product model
        public Product CoolProduct { get; set; } = new Product();

        public Product RecProduct1 { get; set; } = new Product();
        public Product RecProduct2 { get; set; } = new Product();
        public Product RecProduct3 { get; set; } = new Product();

        public string Product_ID { get; set; }


        public ItemRecommendations ItemRecommendations { get; set; } = new ItemRecommendations();
        


        // This is the variable for the return url
        public string ReturnUrl { get; set; } = "/";

        // This is the constructor for our page instantiating the repo
        public ProductDetailsModel(IProductRepository repo,ILogger<ProductDetailsModel> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // This is the get method for the page
        public void OnGet(string returnUrl,string Product_ID)
        {
            ReturnUrl = returnUrl ?? "/";
            CoolProduct = _repo.Products.FirstOrDefault(x => x.Product_ID == Product_ID);

            ItemRecommendations = _repo.ItemRecommendations.FirstOrDefault(x => x.Product_ID == Product_ID);

            RecProduct1 = _repo.Products.FirstOrDefault(x => x.Product_ID == ItemRecommendations.Recommendation_1);
            RecProduct2 = _repo.Products.FirstOrDefault(x => x.Product_ID == ItemRecommendations.Recommendation_2);
            RecProduct3 = _repo.Products.FirstOrDefault(x => x.Product_ID == ItemRecommendations.Recommendation_3);


        }


    }
}
