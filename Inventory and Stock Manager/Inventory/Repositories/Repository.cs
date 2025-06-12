using Inventory.Contexts;
using Inventory.Interfaces;

namespace Inventory.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly InventoryContext context;

        public Repository(InventoryContext c)
        {
            context = c;
        }
        public async Task<T> Add(T item)
        {
            context.Add(item);
            await context.SaveChangesAsync();
            return item;
        }
        public abstract Task<T> GetById(K key);
        public abstract Task<T> GetByName(string s);

        public abstract Task<IEnumerable<T>> GetAll();

    }
}