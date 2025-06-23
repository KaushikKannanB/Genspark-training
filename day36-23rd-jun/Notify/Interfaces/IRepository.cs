using Notify.Models;
namespace Notify.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> Add(T item);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string mail);
    }
}