

using MainMigration.Models;

namespace MainMigration.Interfaces
{
    public interface ITokenService
    {
        Task<string> TokenGenerator(User user);
    }
}