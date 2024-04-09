using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WinterIntex.Models;
using WinterIntex.Infrastructure;
using Microsoft.AspNetCore.Authorization;



namespace WinterIntex.Pages
{
    [Authorize]

    public class CartModel : PageModel
    {
        private IProductRepository _repo;
        public LineItems Cart { get; set; }

        public CartModel(IProductRepository temp, LineItems cartService)
        {
            _repo = temp;
            Cart = cartService;

        }
        public string ReturnUrl { get; set; } = "/";


        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(string projectId, string returnUrl)
        {
            Product proj = _repo.Products
                .FirstOrDefault(x => x.Product_ID == projectId);

            if (proj != null)
            {
                Cart.AddItem(proj, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });

        }

        public IActionResult OnPostRemove(string projectId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.Product_ID == projectId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
