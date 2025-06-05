namespace FirstAPI.Models.DTOs.DoctorSpecialities
{
    public class AppointAddRequest
    {
        public string PatientEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DocName { get; set; } = string.Empty;
        public DateTime Favorable_appointment_time{ get; set; }

    }
}