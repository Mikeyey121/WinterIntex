using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class Country
    {
        [Key]
        public int Country_code { get; set; }
        public string Country_Name { get; set; }
    }
}
