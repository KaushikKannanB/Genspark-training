using Bank.Contexts;
using Bank.Interfaces;
using Bank.Models;
using Bank.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace Bank.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BankContext context;
        public AdminRepository(BankContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Admin>> GetAllAdmins()
        {
            return await context.Admins
                .ToListAsync();
        }
        public async Task<string> AddAdmin(Admin admin)
        {

            await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();
            var u = await GetAdminByMail(admin.Email);
            return u.Id;
        }
        
        public async Task<Admin> GetAdminById(string id)
        {
            return await context.Admins
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<Admin> GetAdminByMail(string mail)
        {
            var user = context.Admins.FirstOrDefaultAsync(u => u.Email == mail);
            return await user;
        }
    }
}