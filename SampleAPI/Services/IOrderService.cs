using SampleAPI.Entities;

namespace SampleAPI.Services
{
    public interface IOrderService
    {
        Task<Order> SubmitOrder(Order orderRequest);
        Task<IEnumerable<Order>> GetRecentOrders(int days);
        Task<bool> DeleteOrder(int id);
        Task<IEnumerable<Order>> GetOrdersInWorkingDays(int days);
    }
}
