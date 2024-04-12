using System.ComponentModel.DataAnnotations;

// Model for the color table that links to the product table primary and secondary color

namespace WinterIntex.Models
{
    public class Color
    {
        [Key]
        public byte Color_Code { get; set; }
        public string Color_Description { get; set; }
    }
}
