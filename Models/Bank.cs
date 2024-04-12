using System.ComponentModel.DataAnnotations;

// Model for the bank table that links to the orders table

namespace WinterIntex.Models
{
    public class Bank
    {
        [Key]
        public int Bank_Code { get; set; }
        public string Bank_Name { get; set; }
    }
}
