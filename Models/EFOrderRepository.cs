using Microsoft.EntityFrameworkCore;

// EF Repository for the ORDERS interface

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

        public IQueryable<EntryMethods> EntryMethods => _context.EntryMethods;
        public IQueryable<TransactionTypes> TransactionTypes => _context.TransactionTypes;
        public IQueryable<Country> Country => _context.Country;
        public IQueryable<Country> ShippingCountry => _context.Country;
        public IQueryable<Bank> Banks => _context.Banks;
        public IQueryable<CardTypes> CardTypes => _context.CardTypes;
        public IQueryable<Customer> Customer => _context.Customer;

        public void SaveOrder(Order order)
        {
            
            _context.Order.Add(order);
            
            _context.SaveChanges();
        }
    }
}