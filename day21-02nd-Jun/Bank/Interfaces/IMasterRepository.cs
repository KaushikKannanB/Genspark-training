using Bank.Models;
namespace Bank.Interfaces
{
    public interface IMasterRepository
    {
        Task<Master> Get(string key);
        Task<string> AddMaster(Master m);
        Task<IEnumerable<Master>> GetAllMaster();
    }
}