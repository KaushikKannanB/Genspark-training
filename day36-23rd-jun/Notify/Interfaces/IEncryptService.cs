using Notify.Models;

namespace Notify.Interfaces
{
    public interface IEncryptService
    {
        Task<EncryptModel> EncryptData(EncryptModel model);
    }
}