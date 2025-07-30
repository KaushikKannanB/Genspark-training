using MainMigration.Context;
using MainMigration.Interfaces;

namespace MainMigration.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly MainMigrationContext context;
        public Repository(MainMigrationContext c)
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
        public abstract Task<IEnumerable<T>> GetAll();
    }
}