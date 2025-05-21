using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cardiologist.Models;

namespace Cardiologist.Interfaces
{
    public interface IPatientService
    {
        int AddAppointment(Appointment appointment);
        List<Appointment> SearchAppointment(SearchModel searchmodel);
    }
}