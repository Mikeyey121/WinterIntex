using System.ComponentModel.DataAnnotations;

// Model for the cardtypes table that links to the orders table

namespace WinterIntex.Models
{
    public class CardTypes
    {
        [Key]
        public int Card_Code { get; set; }
        public string Card_Description { get; set; }
    }
}
