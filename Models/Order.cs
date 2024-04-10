using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using static WinterIntex.Models.Cart;

namespace WinterIntex.Models
{
    public class Order
    {
        [Key]
        public int transaction_ID { get; set; }
        public int customer_ID { get; set; }
        public string? Date { get; set; } 
        public int? Day_Of_Week { get; set; }
        public int? Time { get; set; }
        public int? Entry_Mode { get; set; }
        public int? Amount { get; set; }
        public int? Type_Of_Transaction { get; set; }
        public int? Country_Of_Transaction { get; set; }
        public int? Shipping_Address { get; set; }
        public int? Bank { get; set; }
        public int? Type_Of_Card { get; set; }
        public bool Fraud { get; set; } = false;
    }
}
