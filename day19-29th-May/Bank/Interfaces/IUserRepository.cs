using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUserById(string id);

        public Task<string> AddUser(User user);
        public Task<User> GetUserByMail(string email);
    }
}