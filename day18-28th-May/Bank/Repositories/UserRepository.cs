using Bank.Contexts;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace Bank.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BankContext context;
        public UserRepository(BankContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }
        public async Task<int> AddUser(User User)
        {

            await context.Users.AddAsync(User);
            await context.SaveChangesAsync();
            var u = await GetUserByMail(User.Email);
            return u.Id;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return await user;
        }
        public async Task<User> GetUserByMail(string mail)
        {
            var user = context.Users.FirstOrDefaultAsync(u => u.Email == mail);
            return await user;
        }
    }
}