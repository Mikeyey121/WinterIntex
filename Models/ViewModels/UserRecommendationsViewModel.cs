using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WinterIntex.Models.ViewModels
{
    public class UserRecommendationsViewModel
    {

        // This is the variable for the product model
        public List<Product> RecommendedProducts { get; set; } = new List<Product>();
        public List<TopRecommendations> TopRecommendations { get; set; } = new List<TopRecommendations>();

        public string Product_ID { get; set; }
        public UserRecommendations UserRecommendations { get; set; } 

        // This is the variable for the return url
        public string ReturnUrl { get; set; } = "/";

        public bool IsAuthenticated { get; set; }



    }
    }

