
using Bank.Models.DTOs;
namespace Bank.Services
{
    public interface IFAQServices
    {
        // Task<int> AddInteraction(UserSpecificDTO request);
        Task<string> GetGeneralQueries(UserSpecificDTO request);
        Task<string> GetUserSpecificQueries(UserSpecificDTO request);
    }
}