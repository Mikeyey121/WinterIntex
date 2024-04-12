using System.ComponentModel.DataAnnotations;

// Model for the top recommendations
// We run the model to get the top recommendations for unathenticated users and access it through this model
namespace WinterIntex.Models
{
    public class TopRecommendations
    {
        [Key]
        public string Product_ID { get; set; }
        public double Average_Rating { get; set; }
    }
}
