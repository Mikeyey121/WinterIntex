using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;

// This is the component for the cart summary
namespace WinterIntex.Components
{
    public class CartSummaryViewComponent : ViewComponent 
    {
        private Cart cart; 

        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
