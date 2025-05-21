using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cardiologist.Models;
namespace Cardiologist.Interfaces
{
    public interface IRepositor<K, T> where T : class
    {
        T Add(T item);
        T GetById(K id);
        ICollection<T> GetAll();
    }
}