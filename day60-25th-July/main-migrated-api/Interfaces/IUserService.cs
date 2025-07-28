
using MainMigration.Models;

namespace MainMigration.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByUserName(string username);
    }
}