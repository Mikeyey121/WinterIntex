using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class Category
    {
        [Key]
        public int Category_Number { get; set; }
        public string Category_Description { get; set; } = string.Empty;

        public ICollection<CategoryProductOrder> CategoryProductOrders { get; set; }
    }
}
