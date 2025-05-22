using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMonitoringSOLID.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string department { get; set; }
        public double cgpa { get; set; }


        public Student()
        {

        }

        public Student(int id, string name, string dept, double cgpa)
        {
            Id = id;
            Name = name;
            department = dept;
            this.cgpa = cgpa;
        }

        // public void TakeInputFromUser()
        // {
        //     Console.WriteLine("Enter Student ID:");
        //     int id;
        //     while (!int.Tryparse(Console.ReadLine(), out id))
        //     {
        //         Console.WriteLine("Enter a valid number as ID");
        //     }
        //     Console.WriteLine("Enter Student Name:");
        //     string name = Console.ReadLine() ?? "";
        //     Console.WriteLine("Enter Student Department");
        //     string dept = Console.ReadLine();
        //     Console.WriteLine("Enter Student Department");
        //     double cgpa;
        //     while (!double.TryParse(Console.ReadLine(), out cgpa))
        //     {
        //         Console.WriteLine("Enter a valid value as CGPA");

        //     }

        //     Id = id;
        //     Name = name;
        //     department = dept;
        //     this.cgpa = cgpa;
        // }
        public override string ToString()
        {
            return $"StudentId:{Id}\n StudentName:{Name}\n Department:{department}\n CGPA:{cgpa}";
        }
    }
}