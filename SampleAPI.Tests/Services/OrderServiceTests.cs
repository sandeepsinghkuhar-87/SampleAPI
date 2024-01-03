using Moq;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using SampleAPI.Services;

namespace SampleAPI.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public void SubmitOrder_ShouldCreateNewOrder()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var service = new OrderService(mockRepository.Object);

            var orderRequest = new Order
            {
                Description = "Test Description",
                Name = "Test Name",
            };

            mockRepository.Setup(repo => repo.Create(It.IsAny<Order>()).Result)
                          .Returns((Order order) =>
                          {
                              order.OrderId = 1; // Assuming Id is generated
                              order.OrderDate = DateTime.Now; // Simulate entry date
                              return order;
                          });

            // Act
            var createdOrder = service.SubmitOrder(orderRequest).Result;

            // Assert
            Assert.NotNull(createdOrder);
            Assert.Equal(orderRequest.Description, createdOrder.Description);
            Assert.Equal(orderRequest.Name, createdOrder.Name);
        }

        [Fact]
        public void GetRecentOrders_ShouldReturnRecentOrders()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var service = new OrderService(mockRepository.Object);

            var orders = new List<Order>
            {
                new Order { OrderDate = DateTime.Now.AddDays(-2) },
                new Order { OrderDate = DateTime.Now.AddDays(-1) },
                new Order { OrderDate = DateTime.Now }
            };

            mockRepository.Setup(repo => repo.GetRecentOrders(1).Result).Returns(orders.GetRange(1, 2));

            // Act
            var recentOrders = service.GetRecentOrders(1).Result;

            // Assert
            Assert.NotNull(recentOrders);
            Assert.Equal(2, recentOrders.Count());
        }

        [Fact]
        public void DeleteOrder_ExistingOrderId_ShouldReturnTrue()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var service = new OrderService(mockRepository.Object);

            var orderId = 1;
            mockRepository.Setup(repo => repo.GetById(orderId).Result).Returns(new Order { OrderId = orderId });

            // Act
            var isDeleted = service.DeleteOrder(orderId).Result;

            // Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteOrder_NonExistentOrderId_ShouldReturnFalse()
        {
            // Arrange
            var mockRepository = new Mock<IOrderRepository>();
            var service = new OrderService(mockRepository.Object);

            var orderId = 1;
            mockRepository.Setup(repo => repo.GetById(orderId).Result).Returns((Order)null);

            // Act
            var isDeleted = service.DeleteOrder(orderId).Result;

            // Assert
            Assert.False(isDeleted);
        }

        [Fact]
        public void GetOrdersInWorkingDays_ShouldReturnOrders_ExcludingWeekends()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var orders = new List<Order>
            {
                new Order { OrderId = 1, OrderDate = DateTime.Now.AddDays(-4) },
                new Order { OrderId = 2, OrderDate = DateTime.Now.AddDays(-3) },
                new Order { OrderId = 3, OrderDate = DateTime.Now.AddDays(-2) },
                new Order { OrderId = 4, OrderDate = DateTime.Now.AddDays(-1) },
                new Order { OrderId = 5, OrderDate = DateTime.Now }
            };
            mockOrderRepository.Setup(x => x.GetOrdersSubmittedAfterDate(It.IsAny<DateTime>()).Result).Returns(orders);

            var orderService = new OrderService(mockOrderRepository.Object);

            // Act
            var result = orderService.GetOrdersInWorkingDays(5).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, ((List<Order>)result).Count);
        }
        
    }
}
