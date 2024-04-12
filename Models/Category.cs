using System.ComponentModel.DataAnnotations;

namespace WinterIntex.Models
{
    public class Category
    {
        public Category() 
        {
            CategoryProductOrders = new List<CategoryProductOrder>();

        }
        [Key]
        public int Category_Number { get; set; }
        public string Category_Description { get; set; } = string.Empty;

        public List<CategoryProductOrder> CategoryProductOrders { get; set; }


    }
}
