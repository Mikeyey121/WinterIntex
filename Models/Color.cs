using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class Color
    {
        [Key]
        public byte Color_Code { get; set; }
        public string Color_Description { get; set; }
    }
}
