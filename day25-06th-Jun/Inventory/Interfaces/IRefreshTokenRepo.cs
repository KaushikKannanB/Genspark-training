using Inventory.Models;
namespace Inventory.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetTokenAsync(string token);
        Task InvalidateTokenAsync(RefreshToken token);
    }
}
