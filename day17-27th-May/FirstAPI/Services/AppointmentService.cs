using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Repositories;

namespace FirstAPI.Services
{
    public class AppointmentServices
    {
        private readonly IAppointmentRepository repository;
        public AppointmentServices(IAppointmentRepository repo)
        {
            repository = repo;
        }

        public int AddAppointment(Appointment app)
        {
            var a = repository.GetAppointmentById(app.Id);
            if (a != null)
            {
                return repository.AddAppointment(app);
            }
            return -1;
        }

        public List<Appointment> GetAppointments()
        {
            return repository.GetAppointments();
        }

        public void UpdateAppointment(int id, Appointment app)
        {
            if (!(repository.GetAppointmentById(id) == null))
            {
                repository.UpdateAppointment(id, app);
            }
        }
        public void DeleteAppointment(int id)
        {
            if (!(repository.GetAppointmentById(id) == null))
            {
                repository.DeleteAppointment(id);
            }
        }
        public Appointment GetAppointmentById(int id)
        {
            return repository.GetAppointmentById(id);
        }

        public bool GetAppointmentByIds(int patid, int docid)
        {
            return repository.GetAppointmentByIds(patid, docid);
        }
    }
}