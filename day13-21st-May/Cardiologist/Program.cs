using Cardiologist.Interfaces;
using Cardiologist.Models;
using Cardiologist.Repositories;
using Cardiologist.Services;

namespace Cardiologist
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IRepositor<int, Appointment> appRepository = new PatientRepository();
            IPatientService patService = new PatientServices(appRepository);
            ManageAppointments manageapp = new ManageAppointments(patService);
            manageapp.Start();
        }
    }
}