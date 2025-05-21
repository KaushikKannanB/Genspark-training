using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardiologist.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public Appointment()
        {
            
        }
        public Appointment(int id, string name, int age, DateTime appointmentdate, string reason)
        {
            Id = id;
            PatientName = name;
            PatientAge = age;
            AppointmentDate = appointmentdate;
            Reason = reason;
        }
        public void TakeInputFromUser()
        {
            Console.WriteLine("Enter Patient ID");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Enter a valid Number for ID");
            }
            Console.WriteLine("Enter a patient name");
            string name = Console.ReadLine() ?? "";
            int age;
            Console.WriteLine("Enter Patient Age");
            while (!int.TryParse(Console.ReadLine(), out age))
            {
                Console.WriteLine("Enter a valid Number for ID");
            }
            DateTime date;
            Console.WriteLine("Enter appointment date");

            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Enter a valid date");
            }
            Console.WriteLine("Enter the purpose of visit");
            string reason = Console.ReadLine() ?? "";


            Id = id;
            PatientName = name;
            PatientAge = age;
            AppointmentDate = date;
            Reason = reason;
        }
        public override string ToString()
        {
            return $"Patiend ID:{Id}\nPatient Name:{PatientName}\nPatient Age:{PatientAge}\nAppointment date:{AppointmentDate}\nPurpose:{Reason}";
        }
    }
}