using SampleAPI.Entities;
using SampleAPI.Repositories;

namespace SampleAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> SubmitOrder(Order orderRequest)
        {
            return await _orderRepository.Create(orderRequest);
        }

        public async Task<IEnumerable<Order>> GetRecentOrders(int days)
        {
            return await _orderRepository.GetRecentOrders(days);
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order != null)
            {
                order.DeleteStatus = true;
                await _orderRepository.Update(order);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Order>> GetOrdersInWorkingDays(int days)
        {
            DateTime startDate = CalculateStartDate(days);

            return await _orderRepository.GetOrdersSubmittedAfterDate(startDate);
        }

        private DateTime CalculateStartDate(int days)
        {
            DateTime currentDate = DateTime.Today;
            DateTime startDate = currentDate.AddDays(-days);

            if (startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                startDate = startDate.AddDays(-1);
            }
            else if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                startDate = startDate.AddDays(-2);
            }

            return startDate;
        }
    }
}
