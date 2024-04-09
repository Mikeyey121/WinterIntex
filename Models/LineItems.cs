namespace WinterIntex.Models
{
    public class LineItems
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product p, int quantity)
        {
            CartLine? line = Lines
                .Where(x => x.Product.Product_ID == p.Product_ID)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine()
                {
                    Product = p,
                    qty = quantity
                });
            }
            else
            {
                line.qty += quantity;
            }
        }

        public virtual void RemoveLine(Product proj) => Lines.RemoveAll(x => x.Product.Product_ID == proj.Product_ID);

        public virtual void Clear() => Lines.Clear();

        public decimal CalculateTotal() => Lines.Sum(x => 25 * x.qty);

        public class CartLine
        {
            public int transaction_Id { get; set; }
            public Product Product { get; set; }
            public int qty { get; set; }
            public int rating { get; set; }

        }

    }
}
