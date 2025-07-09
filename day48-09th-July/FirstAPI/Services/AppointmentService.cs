using System.Linq.Expressions;
using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ClinicContext context;
        private readonly IAuthenticationService authenticationService;
        private readonly IPatientService patientService;
        private readonly IDoctorService docService;
        private readonly IRepository<string, Appointmnet> AppointmentRepository;
        private readonly IRepository<int, Patient> PatientRepository;
        public AppointmentService(IDoctorService d, ClinicContext c, IRepository<string, Appointmnet> a, IAuthenticationService aa, IRepository<int, Patient> p, IPatientService pp)
        {
            context = c;
            AppointmentRepository = a;
            authenticationService = aa;
            PatientRepository = p;
            patientService = pp;
            docService = d;
        }
        public async Task<Appointmnet> AddAppointment(AppointAddRequest request)
        {
            UserLoginRequest user = new();
            user.Username = request.PatientEmail;
            user.Password = request.Password;
            var login_response = await authenticationService.Login(user);
            if (login_response.Token != null)
            {
                var p = await patientService.GetPatient(request.PatientEmail);

                var doc = await docService.GetDoctByName(request.DocName);

                var existingAppointments = doc.Appointmnets?.Where(a => a.AppointmnetDateTime == request.Favorable_appointment_time).ToList();

                if (existingAppointments == null)
                {
                    string appno = GenerateAppCode();
                    Appointmnet? a = new();
                    a.AppointmnetNumber = appno;
                    a.PatientId = p.Id;
                    a.DoctorId = doc.Id;
                    a.AppointmnetDateTime = request.Favorable_appointment_time;
                    a.Status = "Booked";

                    return await AppointmentRepository.Add(a);
                }
                else
                {
                    throw new Exception("Doctor is busy!");
                }
            }
            else
            {
                throw new Exception("No such user");
            }
        }
        public static string GenerateAppCode()
        {
            Random random = new Random();
            int number = random.Next(0, 1000); // Generates a number from 0 to 999
            return $"APP{number:D3}"; // Pads with leading zeros to ensure 3 digits
        }
    }
    
}