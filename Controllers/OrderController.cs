using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;

namespace WinterIntex.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService,
                Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("",
                    "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed",
                    new { orderId = order.transaction_ID });
            }
            else
            {
                return View();
            }
        }
    }
}