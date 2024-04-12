using System.ComponentModel.DataAnnotations;

// Model for user reccomendations
// We run our model for user recommendations and post them to the database and then access it through this
namespace WinterIntex.Models
{
    public class UserRecommendations
    {
        [Key]
        public string customer_ID { get; set; }
        public string Recommendation_1 { get; set; }
        public string Recommendation_2 { get; set; }
        public string Recommendation_3 { get; set; }
        public string Recommendation_4 { get; set; }
        public string Recommendation_5 { get; set; }
    }
}