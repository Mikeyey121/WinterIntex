using System.ComponentModel.DataAnnotations;

// Model for the country table that links to any reference of country

namespace WinterIntex.Models
{
    public class Country
    {
        [Key]
        public int Country_code { get; set; }
        public string Country_Name { get; set; }
    }
}
