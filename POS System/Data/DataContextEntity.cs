using Microsoft.EntityFrameworkCore;
using POS_System.Entities;

namespace POS_System.Data
{
    public class DataContextEntity : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

        public DataContextEntity(DbContextOptions<DataContextEntity> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User - Sale relationship
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Cashier)
                .WithMany()
                .HasForeignKey(s => s.CashierId);

            // Configure Sale - SaleProduct relationship
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.SaleProducts)
                .WithOne(sp => sp.Sale)
                .HasForeignKey(sp => sp.SaleId);

            // Configure SaleProduct - Product relationship
            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Product)
                .WithMany()
                .HasForeignKey(sp => sp.ProductId);

            // Configure the primary key and relationships for Product and Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
