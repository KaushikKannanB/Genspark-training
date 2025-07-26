namespace MainMigration.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> Add(T item);
        Task<T> GetById(K key);
        Task<IEnumerable<T>> GetAll();
    }
}
