using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardiologist.Models
{
    public class SearchModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Range<int>? Age { get; set; }
        public DateTime appointmentdate { get; set; }
        public override string ToString()
        {
            return $"Name: {Name}, Age Range: {(Age != null ? $"{Age.MinVal}-{Age.MaxVal}" : "N/A")}, Date: {(appointmentdate != default ? appointmentdate.ToShortDateString() : "N/A")}";
        }

    }
    public class Range<T>
    {
        public T? MinVal { get; set; }
        public T? MaxVal{ get; set; }
    }
}