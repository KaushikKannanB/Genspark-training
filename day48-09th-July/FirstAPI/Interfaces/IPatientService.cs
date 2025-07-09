using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
namespace FirstAPI.Services
{
    public interface IPatientService
    {
        public Task<Patient> AddPatient(PatientAddRequest request);
        public Task<Patient> GetPatient(string email);
    }
}