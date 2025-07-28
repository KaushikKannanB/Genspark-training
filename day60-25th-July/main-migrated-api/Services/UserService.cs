
using MainMigration.Context;
using MainMigration.Interfaces;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Services
{
    public class UserService : IUserService
    {
        private readonly MainMigrationContext context;

        public UserService(MainMigrationContext c)
        {
            context = c;
        }

        public async Task<User> GetByUserName(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(a => a.Username == username);
            return user;
        }
    }
}