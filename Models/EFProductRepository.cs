namespace WinterIntex.Models
{
    // EF Repository for the PRODUCT interface
    public class EFProductRepository : IProductRepository
    {
        // Creating a private variable of type Context file
        private WinterIntexContext _context;

        // Initializing the value of _context in our constructor
        public EFProductRepository(WinterIntexContext temp)
        {
            _context = temp;
        }

        // Setting an IQueryable of type product
        public IQueryable<Product> Products => _context.Products;
        public IQueryable<ItemRecommendations> ItemRecommendations => _context.ItemRecommendations;

        public IQueryable<UserRecommendations> UserRecommendations => _context.UserRecommendations;
        public IQueryable<TopRecommendations> TopRecommendations => _context.TopRecommendations;
        public IQueryable<Color> Color => _context.Color;


        // Method to create a product
        public void CreateProduct(Product p)
        {
            _context.Add(p);
            _context.SaveChanges();
        }

        // Method to delete a product
        public void DeleteProduct(Product p)
        {
            _context.Remove(p);
            _context.SaveChanges();
        }

        // Method to save changes for a product
        public void SaveProduct(Product p)
        {
            _context.Update(p);
            _context.SaveChanges();
        }
    }
}
