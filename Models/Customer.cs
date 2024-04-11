using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinterIntex.Models
{
    public class Customer
    {
        [Key]
        public string customer_ID { get; set; }
        public string first_name { get; set; }
        public DateOnly birth_date { get; set; }
        [ForeignKey("Country")]
        public int country_of_residence { get; set; }
        public int Gender { get; set; }
        public double Age { get; set; }



        public virtual Country? Country { get; set; }

    }
}
