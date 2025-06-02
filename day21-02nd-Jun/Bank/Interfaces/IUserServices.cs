using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Interfaces
{
    public interface IUserService
    {
        Task<string> AddUser(UserRequestDTO user);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> GetUserByMail(string email);
    }
}

