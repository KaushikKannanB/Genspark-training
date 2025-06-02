using Bank.Models;
namespace Bank.Interfaces
{
    public interface IFAQRepository
    {
        Task<int> AddInteraction(FAQ request);
        Task<IEnumerable<FAQ>> GetInteractionById(string id);
        Task<IEnumerable<FAQ>> GetAllInteractions();
    }
}