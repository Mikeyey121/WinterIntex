using Microsoft.AspNetCore.Mvc;
using WinterIntex.Models;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WinterIntex.Controllers
{
    public class OrderController : Controller
    {
        // Global variables for the repository pattern and the logger
        private IOrderRepository _repo;
        private readonly ILogger<OrderController> _logger;

        // Public variable for the cart
        public Cart Cart { get; set; }

        // Public variable for the inference session needed for the onnx prediction
        private readonly InferenceSession _session;

        // Public variable for customer and queryable of entry methods 
        public IQueryable<EntryMethods> EntryMethods { get; set; }
        public Customer Customer { get; set; } = new Customer();

        // Constructor to initialize the repo, cart, and logger
        public OrderController(IOrderRepository repoService,
                Cart cartService,
                ILogger<OrderController> log)
        {
            _repo = repoService;
            Cart = cartService;
            _logger = log;

            _session = new InferenceSession("fraud_detection_model.onnx");
        }

        // Get request for the checkout page
        // We require that the user be logged in for checkout functionality in an attempt to reduce fraud
        [Authorize]
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

        // Post request for the checkout page
        // This will post the order to the database
        // The model prediction will update the order's fraud flag depending on the outcome of the model
        [Authorize]
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
                // ONNX MODEL

                // Dictionary mapping the numeric prediction to a true and false. 
                var class_type_dict = new Dictionary<int, bool>
            {
                { 0, false },
                { 1, true },
            };
                // Storing variables for the datetime to pass to the model
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                int currentDay = DateTime.Now.Day;

                // Logging the order that was passed in and confirming that we have the right customer
                //_logger.LogInformation($"Order customer id being passed in {order.customer_ID}");

                // Assigning the userid variable
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Assigning our customer to what we find in the database based on the userid variable
                Customer = _repo.Customer.FirstOrDefault(x => x.customer_ID == userId);

                // Loggging to ensure that we got a customer
                //_logger.LogInformation($"This is the customer first name {Customer.first_name}");

                // Creating the variable list of type float that we can pass into the model
                // Because of how we normalized the database, we can pass the values directly from the form to the model
                // We also pass the information about the current user for prediction
                var input = new List<float> { (float)order.transaction_ID, (float)order.Day_Of_Week, (float)order.Time, (float)order.Entry_Mode, (float)order.Amount, (float)order.Type_Of_Transaction, (float)order.Country_Of_Transaction, (float)order.Shipping_Address, (float)order.Bank, (float)order.Type_Of_Card, Customer.country_of_residence, (float)Customer.Gender, (float)Customer.Age, currentYear, currentMonth, currentDay, Customer.birth_date.Year, Customer.birth_date.Month, Customer.birth_date.Day };
                
                // Creating the tensor variable
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                // Using the tensor to create an input that can be used by onnx
                var inputs = new List<NamedOnnxValue>
                    {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

                // Runs the prediction based on the inputs
                using (var results = _session.Run(inputs)) {

                    // Logging to see what the results are
                    //_logger.LogInformation($"This is to see what results is {results}");
                    //foreach (var result in results)
                    //{
                    //    // Logging to see what the results are
                    //    _logger.LogInformation($"Result name: {result.Name}, Value: {result.Value}");
                    //}

                    // Creating a variable for the prediction
                    long[] prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    
                    // Logging info for the prediction
                    //_logger.LogInformation($"output_label values: {string.Join(", ", prediction)}");

                    // Creating a boolean value to store the value of the prediction
                    bool Prediction = prediction[0] == 1; // Assuming '1' represents true, and '0' represents false

                    // Logging the variable value
                    //_logger.LogInformation($"bool prediction is: {prediction}");

                    // Dynamically assigning the fraud flag to the database
                    // Dynamically send the user to a completed page based on if it appeared as fraud or not
                    if (Prediction != true)
                        {
                            // Fraud flag is false
                            order.Fraud = false;

                            // Save changes
                            _repo.SaveOrder(order);
                            
                            // Clear the cart
                            Cart.Clear();

                            // Return the user to the regular completed page 
                            return RedirectToPage("/Completed",
                                new { transaction_ID = order.transaction_ID });
                        }
                        else
                        {
                            // Fruad flag is true
                            order.Fraud = true;

                            // Save changes
                            _repo.SaveOrder(order);
                            
                            // Clear the cart
                            Cart.Clear();

                            // Return the user to the fraud completed view
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