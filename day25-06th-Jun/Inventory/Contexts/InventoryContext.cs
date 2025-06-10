using Microsoft.EntityFrameworkCore;
using Inventory.Models;
namespace Inventory.Contexts
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventories> Inventories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StockLogging> StockLogs { get; set; }
        public DbSet<ProductUpdateLog> ProductUpdateLogs { get; set; }
        public DbSet<CategoryAddRequest> CategoryAddRequests { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Admin>().HasKey(a => a.Id);
            modelBuilder.Entity<Manager>().HasKey(m => m.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Inventories>().HasKey(i => i.Id);
            modelBuilder.Entity<ProductUpdateLog>().HasKey(p => p.Id);
            modelBuilder.Entity<StockLogging>().HasKey(s => s.Id);
            modelBuilder.Entity<CategoryAddRequest>().HasKey(ca => ca.Id);


            modelBuilder.Entity<Product>().HasOne(p => p.Inventory).WithOne(i => i.Product).HasForeignKey<Product>(p => p.InventoryId).HasConstraintName("fk_prod_inv");
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).HasConstraintName("fk_prod_cat");
            modelBuilder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products).HasForeignKey(p => p.AddedBy).HasConstraintName("fk_prod_user");

            modelBuilder.Entity<Category>().HasOne(c => c.User).WithMany(u => u.Categories).HasForeignKey(c => c.CreatedBy).HasConstraintName("fk_cat_user");

            modelBuilder.Entity<CategoryAddRequest>().HasOne(ca => ca.User).WithMany(u => u.CategoryAdds).HasForeignKey(ca => ca.RequestedBy).HasConstraintName("fk_category_add_user");

            modelBuilder.Entity<ProductUpdateLog>().HasOne(pl => pl.User).WithMany(u => u.ProductLogs).HasForeignKey(pl => pl.UpdatedBy).HasConstraintName("fk_productLogs_user");

            modelBuilder.Entity<StockLogging>().HasOne(s => s.User).WithMany(u => u.StockLogs).HasForeignKey(s => s.UpdatedBy).HasConstraintName("fk_stockLogs_user");

            modelBuilder.Entity<Admin>().HasOne(a => a.User)
                                        .WithOne(u => u.Admin)
                                        .HasForeignKey<Admin>(a => a.Id)
                                        .HasConstraintName("FK_User_Admin")
                                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Manager>().HasOne(m => m.User)
                                        .WithOne(u => u.Manager)
                                        .HasForeignKey<Manager>(m => m.Id)
                                        .HasConstraintName("FK_User_Manager")
                                        .OnDelete(DeleteBehavior.Restrict);

        }

    }
}