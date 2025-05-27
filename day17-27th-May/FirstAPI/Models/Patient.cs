using System;

namespace FirstAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int age { get; set; }

        // public ICollection<Appointment>? Appointmnets { get; set; }

    }
}
