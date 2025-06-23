using Notify.Models;

namespace Notify.Interfaces
{
    public interface ITokenService
    {
        Task<string> TokenGenerator(User user);
    }
}