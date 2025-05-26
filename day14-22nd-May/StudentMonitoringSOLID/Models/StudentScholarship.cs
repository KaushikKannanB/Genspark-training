using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMonitoringSOLID.Models
{
    public class ScholarshipStudent : Student
    {
        public double ScholarshipAmount { get; set; }

        public ScholarshipStudent() { }

        public ScholarshipStudent(int id, string name, string dept, double cgpa, double scholarshipAmount)
            : base(id, name, dept, cgpa)
        {
            ScholarshipAmount = scholarshipAmount;
        }

        public override string ToString()
        {
            return base.ToString() + $"\n ScholarshipAmount: {ScholarshipAmount}";
        }
    }
}
