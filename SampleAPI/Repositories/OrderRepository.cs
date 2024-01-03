using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;

namespace SampleAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> GetById(int id)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id && o.DeleteStatus == false);
        }

        public async Task<IEnumerable<Order>> GetRecentOrders(int days)
        {
            var thresholdDate = DateTime.Now.AddDays(-days);
            return await _dbContext.Orders.Where(o => o.OrderDate >= thresholdDate && o.DeleteStatus == false).ToListAsync();
        }

        public async Task<Order> Create(Order order)
        {
            order.OrderDate = DateTime.Now;
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task Update(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersSubmittedAfterDate(DateTime startDate)
        {
            return await _dbContext.Orders.Where(o => o.OrderDate > startDate && o.OrderDate <= DateTime.Now && o.DeleteStatus == false).ToListAsync();
        }
    }
}
