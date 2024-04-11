using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class TopRecommendations
    {
        [Key]
        public string Product_ID { get; set; }
        public double Average_Rating { get; set; }
    }
}
