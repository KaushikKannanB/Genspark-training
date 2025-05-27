using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IAppointmentRepository
    {
        int AddAppointment(Appointment appointment);
        List<Appointment> GetAppointments();
        void UpdateAppointment(int id, Appointment appointment);
        void DeleteAppointment(int id);
        Appointment GetAppointmentById(int id);

        bool GetAppointmentByIds(int patid, int docid);
        
    }
}