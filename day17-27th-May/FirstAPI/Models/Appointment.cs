using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = string.Empty;

        [ForeignKey("DoctorId")]
        public Doctor? Doc { get; set; }
        [ForeignKey("PatientId")]
        public Patient? Pat { get; set; }
    }
}