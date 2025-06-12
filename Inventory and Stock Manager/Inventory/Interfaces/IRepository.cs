namespace Inventory.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> GetById(K key);
        Task<T> Add(T item);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByName(string s);
    }
}