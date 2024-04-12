using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// This is the model for the product table
// This contains all of the columns for the table
// It also has built in error messages to inform the user
namespace WinterIntex.Models
{
    public class Product
    {
     
        [Key]
        public string Product_ID { get; set; }


        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please specify how many parts there are")]
        public int num_parts { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Please enter a positive price")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Please specify a link for the image")]
        public string img_Link { get; set; }

        [ForeignKey("PrimaryColor")]
        [Required(ErrorMessage = "Please specify a primary color")]
        public byte Primary_Color { get; set; }

        [ForeignKey("SecondaryColor")]
        [Required(ErrorMessage = "Please specify a secondary color")]
        public byte Secondary_Color { get; set; }
        [Required(ErrorMessage = "Please specify a description of the product")]
        public string Description { get; set; }

        public virtual Color? PrimaryColor { get; set; }
        public virtual Color? SecondaryColor { get; set; }

        public List<CategoryProductOrder>? CategoryProductOrders { get; set; }


    }
}
