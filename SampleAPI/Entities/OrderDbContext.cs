using Microsoft.EntityFrameworkCore;

namespace SampleAPI.Entities
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext() { }
        public OrderDbContext(DbContextOptions<OrderDbContext> options) :
            base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<ExceptionLog>().HasKey(o => o.LogId);

        }

        public virtual DbSet<Order> Orders { get; set; } = null!;
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
    }
}
