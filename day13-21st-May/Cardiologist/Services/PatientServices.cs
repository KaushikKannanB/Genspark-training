using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cardiologist.Interfaces;
using Cardiologist.Models;

namespace Cardiologist.Services
{
    public class PatientServices : IPatientService
    {
        IRepositor<int, Appointment> appointmentRepo;

        public PatientServices(IRepositor<int, Appointment> appointments)
        {
            appointmentRepo = appointments;
        }

        public int AddAppointment(Appointment app)
        {
            try
            {
                var result = appointmentRepo.Add(app);
                if (result != null)
                {
                    return result.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }
        public List<Appointment>? SearchAppointment(SearchModel searchmodel)
        {
            try
            {
                var appointments = appointmentRepo.GetAll();
                appointments = SearchByName(appointments, searchmodel.Name);
                appointments = SearchByAge(appointments, searchmodel.Age);
                appointments = SearchByDate(appointments, searchmodel.appointmentdate);

                if (appointments != null && appointments.Count > 0)
                {
                    return appointments.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new List<Appointment>();
            
        }
        private ICollection<Appointment> SearchByAge(ICollection<Appointment> appointments, Range<int>? age)
        {
            if (age == null || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.PatientAge >= age.MinVal && e.PatientAge <= age.MaxVal).ToList();
            }
        }

        private ICollection<Appointment> SearchByName(ICollection<Appointment> appointments, string? name)
        {
            if (name == null || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.PatientName.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        private ICollection<Appointment> SearchByDate(ICollection<Appointment> appointments, DateTime date)
        {
            if (date == default || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.AppointmentDate == date).ToList();
            }
        }
 
    }
}