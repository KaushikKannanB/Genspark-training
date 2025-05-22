using WholeApplication.Exceptions;
using WholeApplication.Interfaces;
using WholeApplication.Models;

namespace WholeApplication.Repositories
{

    public class EmployeeRepository : Repository<int, Employee>
    {
        protected override int GenerateID()
        {
            return _items.Count > 0 ? _items.Max(e => e.Id) + 1 : 1;
        }

        public override Employee GetById(int id)
        {
            return _items.FirstOrDefault(e => e.Id == id);
        }

        public override ICollection<Employee> GetAll()
        {
            return _items;
        }

        // 👇 You can add more methods here
        public ICollection<Employee> GetByName(string name)
        {
            return _items.Where(e => e.Name.Contains(name)).ToList();
        }
    }
}