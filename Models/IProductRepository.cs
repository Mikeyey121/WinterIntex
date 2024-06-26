﻿namespace WinterIntex.Models
// Interface for the PRODUCT repository
{
    public interface IProductRepository
    {
        // Requiring an attribute of type product, and three methods to allow for crud functionality
        public IQueryable<Product> Products { get; }
        public IQueryable<ItemRecommendations> ItemRecommendations { get; }
        
        public IQueryable<UserRecommendations> UserRecommendations { get; }
        public IQueryable<TopRecommendations> TopRecommendations { get; }
        IQueryable<Color> Color { get; }


        void SaveProduct(Product p);
        void CreateProduct(Product p);
        void DeleteProduct(Product p);
    }
}
