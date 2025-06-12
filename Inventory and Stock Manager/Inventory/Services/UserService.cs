using Inventory.Interfaces;
using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;


namespace Inventory.Services
{
    public class UserService : IUserService
    {
        private readonly InventoryContext context;

        public UserService(InventoryContext c)
        {
            context = c;
        }

        public async Task<User> GetByMail(string mail)
        {
            var user = await context.Users.FirstOrDefaultAsync(a => a.Email == mail);
            return user;
        }
    }
}