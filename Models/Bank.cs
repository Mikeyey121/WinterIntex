using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class Bank
    {
        [Key]
        public int Bank_Code { get; set; }
        public string Bank_Name { get; set; }
    }
}
