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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Balance).IsRequired();

                entity.HasMany(u => u.Transactions).WithOne(t => t.User).HasForeignKey(t => t.UserId);

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