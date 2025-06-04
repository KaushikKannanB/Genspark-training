using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.Contexts
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FAQ> Interactions{ get; set; }
        public DbSet<Admin> Admins{ get; set; }
        public DbSet<Master> Master{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasOne(u => u.Master).WithOne(m=>m.User).HasForeignKey<User>(u=>u.Name).HasConstraintName("FK_User_Master");
                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Balance).IsRequired();

                entity.HasMany(u => u.Transactions).WithOne(t => t.User).HasForeignKey(t => t.UserId);

            });
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).IsRequired();
                entity.Property(a => a.Email).IsRequired();
                entity.HasOne(a => a.Master).WithOne(m => m.Admin).HasForeignKey<Admin>(a=>a.Name).HasConstraintName("FK_Admin_Master");
            });
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Type).IsRequired();
                entity.Property(t => t.Amount).IsRequired();
                entity.Property(t => t.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<FAQ>(entity =>
            {
                entity.HasKey(i => i.Id);
            });
        }
    }
}