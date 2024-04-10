using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class ItemRecommendations
    {
        [Key]
        public string Product_ID { get; set; }
        public string Recommendation_1 { get; set; }
        public string Recommendation_2 { get; set; }
        public string Recommendation_3 { get; set; }
        public string Recommendation_4 { get; set; }
        public string Recommendation_5 { get; set; }

    }
}
