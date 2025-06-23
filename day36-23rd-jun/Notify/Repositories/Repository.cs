using Notify.Models;
using Notify.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notify.Context;
namespace Notify.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly NotifyContext context;
        public Repository(NotifyContext c)
        {
            context = c;
        }

        public async Task<T> Add(T item)
        {
            context.Add(item);
            await context.SaveChangesAsync();
            return item;
        }

        public abstract Task<T> GetById(string mail);

        public abstract Task<IEnumerable<T>> GetAll();

    }
}