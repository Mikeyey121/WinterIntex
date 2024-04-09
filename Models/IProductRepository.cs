namespace WinterIntex.Models
// Interface for our repository
{
    public interface IProductRepository
    {
        // Requiring an attribute of type product, and three methods to allow for crud functionality
        public IQueryable<Product> Products { get; }

        void SaveProduct(Product p);
        void CreateProduct(Product p);
        void DeleteProduct(Product p);
    }
}
