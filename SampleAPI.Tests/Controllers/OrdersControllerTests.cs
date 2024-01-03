using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SampleAPI.Controllers;
using SampleAPI.Entities;
using SampleAPI.Mapping;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using SampleAPI.Response;
using SampleAPI.Services;

namespace SampleAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly IMapper _mockMapper;
        private readonly OrdersController _orderController;
        private readonly Mock<ILogger<OrdersController>> _logger;

        public OrdersControllerTests()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            _mockMapper = configuration.CreateMapper();

            _mockOrderService = new Mock<IOrderService>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _logger = new Mock<ILogger<OrdersController>>();
            _orderController = new OrdersController(_mockOrderService.Object, _mockMapper, _logger.Object);
        }

        [Fact]
        public void SubmitOrder_ShouldReturnSubmittedOrder()
        {
            // Arrange

            _mockOrderRepository.Setup(repo => repo.Create(It.IsAny<Order>()).Result)
                         .Returns((Order order) =>
                         {
                             order.OrderId = 1;
                             order.OrderDate = DateTime.Now;
                             return order;
                         });

            _mockOrderService.Setup(service => service.SubmitOrder(It.IsAny<Order>()).Result)
                         .Returns((Order order) =>
                         {
                             order.OrderId = 1;
                             order.OrderDate = DateTime.Now;
                             return order;
                         });

            var orderRequest = new CreateOrderRequest
            {
                Description = "Test Description",
                Name = "Test Name",
            };

            var expectedOrder = new OrderResponse
            {
                Description = orderRequest.Description,
                Name = orderRequest.Name,
                OrderId = 1
            };

            // Act
            var result = _orderController.SubmitOrder(orderRequest).Result as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var submittedOrder = result.Value as OrderResponse;
            Assert.NotNull(submittedOrder);
            Assert.Equal(expectedOrder.Description, submittedOrder.Description);
            Assert.Equal(expectedOrder.Name, submittedOrder.Name);
        }

        [Fact]

        public void GetRecentOrders_ShouldReturnRecentOrders()
        {
            // Arrange
            
            var orders = new List<Order>
            {
                new Order { OrderDate = DateTime.Now.AddDays(-2) },
                new Order { OrderDate = DateTime.Now.AddDays(-1) },
                new Order { OrderDate = DateTime.Now }
            };

            _mockOrderService.Setup(service => service.GetRecentOrders(1).Result).Returns(orders.Skip(1));

            // Act
            var result = _orderController.GetRecentOrders(1).Result as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var recentOrders = result.Value as IEnumerable<OrderResponse>;
            Assert.NotNull(recentOrders);
            Assert.Equal(2, recentOrders.Count());
        }

        [Fact]
        public void DeleteOrder_ExistingOrderId_ShouldReturnOk()
        {
            // Arrange

            var orderId = 1;
            _mockOrderService.Setup(service => service.DeleteOrder(orderId).Result).Returns(true);

            // Act
            var result = _orderController.DeleteOrder(orderId).Result as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Order deleted successfully", result.Value);
        }

        [Fact]
        public void DeleteOrder_NonExistentOrderId_ShouldReturnNotFound()
        {
            // Arrange
            var orderId = 1;
            _mockOrderService.Setup(service => service.DeleteOrder(orderId).Result).Returns(false);

            // Act
            var result = _orderController.DeleteOrder(orderId).Result as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Order not found", result.Value);
        }
    }
}
