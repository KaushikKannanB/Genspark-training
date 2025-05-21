using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cardiologist.Models;

namespace Cardiologist.Repositories
{
    public class PatientRepository : Repositor<int, Appointment>
    {
        protected override int GenerateID()
        {
            return items.Count > 0 ? items.Max(e => e.Id) + 1 : 1;
        }

        public override Appointment GetById(int id)
        {
            return items.FirstOrDefault(e => e.Id == id);
        }

        public override ICollection<Appointment> GetAll()
        {
            return items;
        }


    }
}