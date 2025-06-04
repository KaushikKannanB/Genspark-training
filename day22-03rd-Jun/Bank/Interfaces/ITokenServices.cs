using Bank.Models;
namespace Bank.Interfaces
{
    public interface ITokenServices
    {
        Task<string> GenerateToken(Master master);
    }
}