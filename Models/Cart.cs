using Microsoft.Build.Evaluation;

// Model for the cart
// Includes all crud functionality 
namespace WinterIntex.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        // CREATE
        public virtual void AddItem(Product p, int quantity)
        {
            CartLine? line = Lines
                .Where(x => x.Product.Product_ID == p.Product_ID)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = p,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        // DELETE
        public virtual void RemoveLine(Product p) => Lines.RemoveAll(x => x.Product.Product_ID == p.Product_ID);

        // DELETE ALL
        public virtual void Clear() => Lines.Clear();

        // CALCULATE TOTAL
        public decimal CalculateTotal() => Lines.Sum(x => x.Product.Price * x.Quantity);


        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
