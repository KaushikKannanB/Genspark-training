using Inventory.Models;

namespace Inventory.Interfaces
{
    public interface IEncryptService
    {
        Task<EncryptModel> EncryptData(EncryptModel e);
        Task<string> GenerateId(string starter);
    }
}