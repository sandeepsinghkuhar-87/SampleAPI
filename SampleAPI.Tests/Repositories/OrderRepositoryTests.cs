using SampleAPI.Entities;
using SampleAPI.Repositories;

namespace SampleAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly OrderDbContext _dbcontext;
        public OrderRepositoryTests()
        {
            _dbcontext = MockSampleApiDbContextFactory.GenerateMockContext();
        }
        [Fact]
        public void Create_ShouldAddNewOrder()
        {
            // Arrange

            var repository = new OrderRepository(_dbcontext);
            var order = new Order
            {
                Description = "Test Order",
                Name = "Test",
            };

            // Act
            var createdOrder = repository.Create(order).Result;

            // Assert
            Assert.NotNull(createdOrder);
            Assert.Equal(order.Description, createdOrder.Description);
            Assert.NotEqual(default(DateTime), createdOrder.OrderDate);

            _ = repository.Delete(createdOrder);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnOrder()
        {

            var repository = new OrderRepository(_dbcontext);
            var orderId = 1;
            var order = new Order
            {
                Description = "Test Order",
                Name = "Test",
            };

            // Act
            repository.Create(order);

            // Act
            var retrievedOrder = repository.GetById(orderId).Result;

            // Assert
            Assert.NotNull(retrievedOrder);
            Assert.Equal(orderId, retrievedOrder.OrderId);

            _ = repository.Delete(retrievedOrder);
        }

        [Fact]
        public void GetRecentOrders_ShouldReturnRecentOrders()
        {
            var repository = new OrderRepository(_dbcontext);
            var orders = Enumerable.Range(1, 5).Select(i => new Order
            {
                Description = $"Test Order {i}",
                Name = $"Test {i}",
                OrderDate = DateTime.Now.AddDays(-i)
            }).ToList();

            foreach (var order in orders)
            {
                repository.Create(order);
            }

            // Act
            var recentOrders = repository.GetRecentOrders(3).Result;

            // Assert
            Assert.NotNull(recentOrders);
            Assert.Equal(5, recentOrders.Count());
        }

        [Fact]
        public void Delete_Order_ShouldBeDeleted()
        {
            // Arrange
            var repository = new OrderRepository(_dbcontext);
            var orderId = 1;
            var order = new Order
            {
                OrderId = orderId,
                Name = "Delete Test",
                Description = "Delete Test"
            };

            _dbcontext.Orders.Add(order);
            _dbcontext.SaveChanges();

            // Act
            order.DeleteStatus = true;
            _ = repository.Update(order);

            // Assert
            var deletedOrder = _dbcontext.Orders.FirstOrDefault(o => o.OrderId == orderId);
            Assert.True(deletedOrder.DeleteStatus);
        }

        [Fact]
        public void GetOrdersSubmittedAfterDate_ShouldReturnOrders()
        {
            // Arrange
            var repository = new OrderRepository(_dbcontext);
            var orders = Enumerable.Range(1, 5).Select(i => new Order
            {
                Description = $"Test Order {i}",
                Name = $"Test {i}",
                OrderDate = DateTime.Now.AddDays(-i)
            }).ToList();

            foreach (var order in orders)
            {
                repository.Create(order);
            }

            var startDate = DateTime.Now.AddDays(-3);
            // Act
            var recentOrders = repository.GetOrdersSubmittedAfterDate(startDate).Result;

            // Assert
            Assert.NotNull(recentOrders);
            Assert.True(recentOrders.Count() >= 5);
        }


    }
}