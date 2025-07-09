using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointmnet> AddAppointment(AppointAddRequest request);
    }
}