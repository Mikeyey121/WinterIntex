using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WinterIntex.Models;

namespace WinterIntex.Pages
{

    public class CartModel : PageModel
    {
        private IProductRepository _repo;
        private readonly ILogger<CartModel> _logger;

        public CartModel(IProductRepository repo, ILogger<CartModel> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public Cart? Cart { get; set; }
        public void OnGet()
        {
        }

        public void OnPost(string Product_ID)
        {
            _logger.LogInformation($"The productId that came into the post is {Product_ID}");
            Product prod = _repo.Products
                .FirstOrDefault(x => x.Product_ID == Product_ID);

            Cart = new Cart();

            _logger.LogInformation("The prod that was created is ", prod);
            Cart.AddItem(prod, 1);
        }
    }
}
