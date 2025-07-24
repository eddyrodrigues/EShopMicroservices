using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;
        public DiscountContext(DbContextOptions<DiscountContext> optionsBuilder) : base(optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon()
                {
                    Id = 1,
                    ProductName = "Product One",
                    Amount = 1,
                    Description = "Desc1"
                },
                new Coupon()
                {
                    Id = 2,
                    ProductName = "Product Tw",
                    Amount = 2,
                    Description = "Desc2"
                }
                );
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=databse.dat");
        //}

    }
}
