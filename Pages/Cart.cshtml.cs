using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WinterIntex.Infrastructure;
using WinterIntex.Models;

namespace WinterIntex.Pages
{

    public class CartModel : PageModel
    {
        private IProductRepository _repo;

        public CartModel(IProductRepository repo, Cart cartService)
        {
            _repo = repo;
            Cart = cartService;
        }
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(string Product_ID,string returnUrl)
        {
            Product prod = _repo.Products
                .FirstOrDefault(x => x.Product_ID == Product_ID);

            if (prod != null)
            {
                Cart.AddItem(prod, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }
        public IActionResult OnPostRemove(string Product_ID,
                string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl =>
                cl.Product.Product_ID == Product_ID).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
