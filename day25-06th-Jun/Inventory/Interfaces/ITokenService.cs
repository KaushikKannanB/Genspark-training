using Inventory.Models;

namespace Inventory.Interfaces
{
    public interface ITokenService
    {
        Task<string> TokenGenerator(User user);
    }
}