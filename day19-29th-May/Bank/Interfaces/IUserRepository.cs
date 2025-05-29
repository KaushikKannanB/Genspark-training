using Bank.Models;
namespace Bank.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUserById(int id);

        public Task<int> AddUser(User user);
        public Task<User> GetUserByMail(string email);
    }
}