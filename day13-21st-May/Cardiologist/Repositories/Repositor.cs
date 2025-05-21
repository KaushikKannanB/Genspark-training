using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cardiologist.Interfaces;
using Cardiologist.Models;
using Cardiologist.Exceptions;

namespace Cardiologist.Repositories
{
    public abstract class Repositor<K, T> : IRepositor<K, T> where T : class
    {
        protected List<T> items = new List<T>();
        protected abstract K GenerateID();

        public abstract ICollection<T> GetAll();

        public abstract T GetById(K id);

        public T Add(T item)
        {
            var id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }
            if (items.Contains(item))
            {
                throw new DuplicateEntityException("Appointment already exists!");
            }
            items.Add(item);

            return item;
        }
    }
}