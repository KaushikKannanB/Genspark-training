using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
namespace FirstAPI.Misc
{
    public class PatientMapper
    {
        public Patient? PatientAddMapper(PatientAddRequest request)
        {
            Patient p = new();
            p.Name = request.Name;
            p.Email = request.Email;
            p.Password = request.Password;
            p.Age = request.Age;
            p.Phone = request.Phone;

            return p;
        }
    }
}