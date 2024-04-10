using Microsoft.EntityFrameworkCore;

namespace WinterIntex.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private WinterIntexContext _context;

        public EFOrderRepository(WinterIntexContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Order> Order => _context.Order;

        public void SaveOrder(Order order)
        {
            
            _context.Order.Add(order);
            
            _context.SaveChanges();
        }
    }
}