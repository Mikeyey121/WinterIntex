using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;

namespace WinterIntex.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {

        private LineItems cart;
        public CartSummaryViewComponent(LineItems cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
