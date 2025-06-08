
using Inventory.Models;
namespace Inventory.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByMail(string mail);
    }
}