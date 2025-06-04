using Bank.Models;
namespace Bank.Interfaces
{
    public interface IEncryptService
    {
        Task<EncryptModel> EncryptData(EncryptModel model);
    }

}