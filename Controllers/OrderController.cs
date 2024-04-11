using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WinterIntex.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository _repo;
        private readonly ILogger<OrderController> _logger;

        public Cart Cart { get; set; }
        private readonly InferenceSession _session;

        public IQueryable<EntryMethods> EntryMethods { get; set; }
        public Customer Customer { get; set; } = new Customer();

        public OrderController(IOrderRepository repoService,
                Cart cartService,
                ILogger<OrderController> log)
        {
            _repo = repoService;
            Cart = cartService;
            _logger = log;

            _session = new InferenceSession("fraud_detection_model.onnx");
        }

        public ViewResult Checkout()
        {
            Order order = new Order();
            ViewBag.EntryMethods = _repo.EntryMethods.ToList();
            ViewBag.Countries = _repo.Country.ToList();
            ViewBag.Banks = _repo.Banks.ToList();
            ViewBag.CardTypes = _repo.CardTypes.ToList();
            ViewBag.MaxTransaction = _repo.Order.Max(x => x.transaction_ID);

            decimal cartTotal = Cart.CalculateTotal(); // Replace 'CalculateTotal()' with your actual method/property

            // Pass the total price to the view using ViewBag
            ViewBag.CartTotal = cartTotal;




            return View(order);
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            ViewBag.EntryMethods = _repo.EntryMethods.ToList();
            ViewBag.Countries = _repo.Country.ToList();
            ViewBag.Banks = _repo.Banks.ToList();
            ViewBag.CardTypes = _repo.CardTypes.ToList();
            ViewBag.MaxTransaction = _repo.Order.Max(x => x.transaction_ID);
            decimal cartTotal = Cart.CalculateTotal(); // Replace 'CalculateTotal()' with your actual method/property

            // Pass the total price to the view using ViewBag
            ViewBag.CartTotal = cartTotal;


            

            if (Cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("",
                    "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                // The recommendation model needs to go HERE



                // EPIC ONNX STUFF

                // Dictionary mapping the numeric prediction to a true and false. 
                var class_type_dict = new Dictionary<int, bool>
            {
                { 0, false },
                { 1, true },
            };

                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                int currentDay = DateTime.Now.Day;
                _logger.LogInformation($"Order customer id being passed in {order.customer_ID}");
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Customer = _repo.Customer.FirstOrDefault(x => x.customer_ID == userId);
                _logger.LogInformation($"This is the customer first name {Customer.first_name}");
                var input = new List<float> { (float)order.transaction_ID, (float)order.Day_Of_Week, (float)order.Time, (float)order.Entry_Mode, (float)order.Amount, (float)order.Type_Of_Transaction, (float)order.Country_Of_Transaction, (float)order.Shipping_Address, (float)order.Bank, (float)order.Type_Of_Card, Customer.country_of_residence, (float)Customer.Gender, (float)Customer.Age, currentYear, currentMonth, currentDay, Customer.birth_date.Year, Customer.birth_date.Month, Customer.birth_date.Day };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
                var inputs = new List<NamedOnnxValue>
                    {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };
                using (var results = _session.Run(inputs)) // makes the prediction with the inputs from the form (i.e. class_type 1-7)
                {
                    _logger.LogInformation($"This is to see what results is {results}");
                    foreach (var result in results)
                    {
                        _logger.LogInformation($"Result name: {result.Name}, Value: {result.Value}");
                    }

                    long[] prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    _logger.LogInformation($"output_label values: {string.Join(", ", prediction)}");

                    
                        bool Prediction = prediction[0] == 1; // Assuming '1' represents true, and '0' represents false

                    _logger.LogInformation($"bool prediction is: {prediction}");

                    if (Prediction != true)
                        {
                            order.Fraud = false;
                            _repo.SaveOrder(order);

                            Cart.Clear();

                            // If statement for the return. 
                            return RedirectToPage("/Completed",
                                new { transaction_ID = order.transaction_ID });
                        }
                        else
                        {
                            order.Fraud = true;
                            _repo.SaveOrder(order);

                            Cart.Clear();
                            return RedirectToPage("/FraudCompleted",
                                new { transaction_ID = order.transaction_ID });
                        }
                    
                    

                }



               
            }
            else
            {
                return View(order);
            }
        }



        

      
    }
}