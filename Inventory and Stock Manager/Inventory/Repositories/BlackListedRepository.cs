using Inventory.Models;
using Inventory.Interfaces;
using Inventory.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class BlacklistedTokenRepository : IBlacklistedTokenRepository
    {
        private readonly InventoryContext _context;

        public BlacklistedTokenRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task AddTokenAsync(BlacklistedToken token)
        {
            await _context.BlacklistedTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _context.BlacklistedTokens
                .AnyAsync(t => t.Token == token && t.ExpiryDate > DateTime.UtcNow);
        }
    }

}