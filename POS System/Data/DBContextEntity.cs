using Microsoft.EntityFrameworkCore;
using POS_System.Entities;

namespace POS_System.Data
{
    public class DBContextEntity : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DBContextEntity(DbContextOptions<DBContextEntity> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User - Sale relationship
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Cashier)
                .WithMany()
                .HasForeignKey(s => s.CashierId);

            // Configure Sale - ProductItem relationship
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Products)
                .WithOne()
                .HasForeignKey(pi => pi.ProductItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Purchase - ProductItem relationship
            modelBuilder.Entity<Purchase>()
                .HasMany(p => p.Products)
                .WithOne()
                .HasForeignKey(pi => pi.ProductItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ProductItem - Product relationship
            modelBuilder.Entity<ProductItem>()
                .HasOne(pi => pi.Product)
                .WithMany()
                .HasForeignKey(pi => pi.ProductId);

            // Configure the primary key and relationships for Product and Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
