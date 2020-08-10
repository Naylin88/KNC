using Microsoft.EntityFrameworkCore;
using InitCMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InitCMS.Data
{
    public class InitCMSContext : IdentityDbContext
    {
        public InitCMSContext(DbContextOptions<InitCMSContext> options)
         : base(options)
        {


        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.PCode)
                .IsUnique()
                .HasFilter("[PCode] is Not Null");
            
            modelBuilder.Entity<User>()
                .HasIndex(e => e.UserEmail)
                .IsUnique()
                .HasFilter("[UserEmail] is Not Null");

        }

    }
}