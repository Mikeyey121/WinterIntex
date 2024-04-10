using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class TransactionTypes
    {
        [Key]
        public int Transaction_Code { get; set; }
        public string Transaction_Description { get; set; }
    }
}
