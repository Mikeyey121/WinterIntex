using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Model for the linking table between orders and categories

namespace WinterIntex.Models
{
    public class CategoryProductOrder
    {
        [Key]
        public int OrderSequence { get; set; }

        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
