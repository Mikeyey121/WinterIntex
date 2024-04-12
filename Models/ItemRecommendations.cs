using System.ComponentModel.DataAnnotations;

// Item recommendation model
// We push the item recommendations to the database and access them on the page through this model

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
