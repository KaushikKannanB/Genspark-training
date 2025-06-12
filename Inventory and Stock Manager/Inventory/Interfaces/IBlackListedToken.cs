using Inventory.Models;
namespace Inventory.Interfaces
{
    public interface IBlacklistedTokenRepository
    {
        Task AddTokenAsync(BlacklistedToken token);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }

}