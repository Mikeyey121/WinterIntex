using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class CardTypes
    {
        [Key]
        public int Card_Code { get; set; }
        public string Card_Description { get; set; }
    }
}
