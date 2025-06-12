using Inventory.Contexts;
using Inventory.Interfaces;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Inventory.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly InventoryContext _context;

        public RefreshTokenRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task InvalidateTokenAsync(RefreshToken token)
        {
            token.IsUsed = true;
            token.IsRevoked = true;
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}
