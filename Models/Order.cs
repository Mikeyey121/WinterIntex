using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static WinterIntex.Models.Cart;

namespace WinterIntex.Models
{
    public class Order
    {
        [Key]
        public int transaction_ID { get; set; }
        public int customer_ID { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public string? Date { get; set; }

        [Required(ErrorMessage = "Day of week is required")]
        public int? Day_Of_Week { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public int? Time { get; set; }

        [Required(ErrorMessage = "Entry_Mode is required")]
        [ForeignKey("EntryMethods")]
        public int? Entry_Mode { get; set; }


        [Required(ErrorMessage = "Amount is required")]
        public int? Amount { get; set; }


        [Required(ErrorMessage = "Transaction types is required")]
        [ForeignKey("TransactionTypes")]
        public int? Type_Of_Transaction { get; set; }


        [Required(ErrorMessage = "Country is required")]
        [ForeignKey("Country")]
        public int? Country_Of_Transaction { get; set; }


        [Required(ErrorMessage = "Shipping Country is required")]
        [ForeignKey("ShippingCountry")]
        public int? Shipping_Address { get; set; }


        [Required(ErrorMessage = "Bank is required")]
        [ForeignKey("Banks")]
        public int? Bank { get; set; }

        [Required(ErrorMessage = "TypeofCard is required")]
        [ForeignKey("CardTypes")]
        public int? Type_Of_Card { get; set; }
        public bool Fraud { get; set; } = false;

        // Foreign key connections
        public virtual EntryMethods? EntryMethods { get; set; }
        public virtual TransactionTypes? TransactionTypes { get; set; }
        public virtual Country? Country { get; set; }
        public virtual Country? ShippingCountry { get; set; }
        public virtual Bank? Banks { get; set; }
        public virtual CardTypes? CardTypes { get; set; }

    }
}
