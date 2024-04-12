using System.ComponentModel.DataAnnotations;

// Model for transaction types that links to the order table
namespace WinterIntex.Models
{
    public class TransactionTypes
    {
        [Key]
        public int Transaction_Code { get; set; }
        public string Transaction_Description { get; set; }
    }
}
