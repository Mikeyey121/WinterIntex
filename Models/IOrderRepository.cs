namespace WinterIntex.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Order { get; }

        IQueryable<EntryMethods> EntryMethods { get; }
        IQueryable<TransactionTypes> TransactionTypes { get; }
        IQueryable<Country> Country { get; }
        IQueryable<Country> ShippingCountry { get; }
        IQueryable<Bank> Banks { get; }
        IQueryable<CardTypes> CardTypes { get; }
        IQueryable<Customer> Customer { get; }
        void SaveOrder(Order order);
    }
}
