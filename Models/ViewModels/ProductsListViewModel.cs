namespace WinterIntex.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public string? CurrentProjectType { get; set; }
    }
}
