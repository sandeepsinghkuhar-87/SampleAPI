using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SampleAPI.Entities;

namespace SampleAPI.Tests
{
    /// <summary>
    /// Mock database provided for your convenience.
    /// </summary>
    internal static class MockSampleApiDbContextFactory
    {
        public static OrderDbContext GenerateMockContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "mock_SampleApiDbContext")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new OrderDbContext(options);
            return context;
        }
    }
}
