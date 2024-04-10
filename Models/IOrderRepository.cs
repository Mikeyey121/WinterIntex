namespace WinterIntex.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Order { get; }
        void SaveOrder(Order order);
    }
}
