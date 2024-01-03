using SampleAPI.Entities;

namespace SampleAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetById(int id);
        Task<IEnumerable<Order>> GetRecentOrders(int days);
        Task<Order> Create(Order order);
        Task Update(Order order);
        Task<IEnumerable<Order>> GetOrdersSubmittedAfterDate(DateTime startDate);
        Task Delete(Order order);
    }
}
