using Microsoft.AspNetCore.Mvc;

using FirstAPI.Models;
using FirstAPI.Interfaces;

namespace FirstAPI.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private List<Appointment> appointments = new List<Appointment>();

        public int AddAppointment(Appointment appointment)
        {
            

            appointments.Add(appointment);
            return appointment.Id;
        }

        public List<Appointment> GetAppointments()
        {
            return appointments;
        }
        public void UpdateAppointment(int id, Appointment app)
        {
            var a = appointments.FirstOrDefault(a => a.Id == id);
            if (a != null)
            {
                a.PatientId = app.PatientId;
                a.DoctorId = app.DoctorId;
                a.AppointmentDate = app.AppointmentDate;
            }
            return;
        }

        public void DeleteAppointment(int id)
        {
            var a = appointments.FirstOrDefault(a => a.Id == id);
            if (a != null)
            {
                appointments.Remove(a);
            }
            return;
        }

        public Appointment GetAppointmentById(int id)
        {
            var a = appointments.FirstOrDefault(a => a.Id == id);
            return a;
        }

        public bool GetAppointmentByIds(int patid, int docid)
        {
            var a = appointments.FirstOrDefault(a => a.PatientId == patid && a.DoctorId==docid);
            return a != null;
        }
    }
}