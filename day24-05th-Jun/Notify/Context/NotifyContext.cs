using Notify.Models;
using Microsoft.EntityFrameworkCore;
namespace Notify.Context
{
    public class NotifyContext : DbContext
    {
        public NotifyContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasOne(m => m.User).WithOne(u => u.Member).HasForeignKey<Member>(m => m.Email).HasConstraintName("FK_MEMBERR_USER");
            modelBuilder.Entity<Admin>().HasOne(m => m.User).WithOne(u => u.Admin).HasForeignKey<Admin>(m => m.Email).HasConstraintName("FK_ADMIN_USER");
        }
    }
}