using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace WinterIntex.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository _repo;
        private Cart cart;
        private readonly InferenceSession _session;

        public IQueryable<EntryMethods> EntryMethods { get; set; }

        public OrderController(IOrderRepository repoService,
                Cart cartService)
        {
            _repo = repoService;
            cart = cartService;

            _session = new InferenceSession("C:\\Users\\Mhutc\\source\\repos\\WinterIntex\\fraud_detection_model.onnx");
        }

        public ViewResult Checkout() {

            ViewBag.EntryMethods = _repo.EntryMethods.ToList();
            ViewBag.Countries = _repo.Country.ToList();
            ViewBag.Banks = _repo.Banks.ToList();
            ViewBag.CardTypes = _repo.CardTypes.ToList();


            return View(new Order());
        } 

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            ViewBag.EntryMethods = _repo.EntryMethods.ToList();
            ViewBag.Countries = _repo.Country.ToList();
            ViewBag.Banks = _repo.Banks.ToList();
            ViewBag.CardTypes = _repo.CardTypes.ToList();

            // EPIC ONNX STUFF

            // Dictionary mapping the numeric prediction to an animal type
            //var class_type_dict = new Dictionary<int, bool>
            //{
            //    { 0, false },
            //    { 1, true },

            //};

            //try
            //{
            //    var input = new List<int?> { order.transaction_ID, order.customer_ID, order.Day_Of_Week, order.Time, order.Amount, order.Type_Of_Transaction, order.Country_Of_Transaction, order.Shipping_Address, order.Bank, order.Type_Of_Card, order.Date.Year, order.Date.Month, order.Date.Day, order.country_of_residence,order.gender,order.age };
            //    var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            //    var inputs = new List<NamedOnnxValue>
            //    {
            //        NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
            //    };

            //    using (var results = _session.Run(inputs)) // makes the prediction with the inputs from the form (i.e. class_type 1-7)
            //    {
            //        var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
            //        if (prediction != null && prediction.Length > 0)
            //        {
            //            // Use the prediction to get the animal type from the dictionary
            //            var animalType = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
            //            ViewBag.Prediction = animalType;
            //        }
            //        else
            //        {
            //            ViewBag.Prediction = "Error: Unable to make a prediction.";
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Prediction = "Error during prediction.";
            //}

            // EPIC ONNX STUFF 


            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("",
                    "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                _repo.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed",
                    new { orderId = order.transaction_ID });
            }
            else
            {
                return View(order);
            }
        }
        
    }
}