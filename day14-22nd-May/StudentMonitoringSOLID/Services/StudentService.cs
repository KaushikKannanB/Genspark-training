using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StudentMonitoringSOLID.Models;
using StudentMonitoringSOLID.Interfaces;
using StudentMonitoringSOLID.Repositories;

namespace StudentMonitoringSOLID.Services
{
    public class StudentService
    {
        private readonly IStudentRepository studentrepo;

        public StudentService(IStudentRepository repository)
        {
            studentrepo = repository;
        }

        public int AddStudent(Student student)
        {
            if (studentrepo.GetStudentById(student.Id) != null)
            {
                Console.WriteLine("Student with this ID already exists");
                return -1;
            }
            return studentrepo.AddStudent(student);
        }

        public void UpdateStudent(Student student)
        {
            var existing = studentrepo.GetStudentById(student.Id);
            if (existing == null)
            {
                Console.WriteLine("No such student exist");
                return;
            }
            studentrepo.UpdateStudent(student);
        }

        public Student GetStudentById(int id)
        {
            return studentrepo.GetStudentById(id);
        }

        public List<Student> GetAllStudents()
        {
            return studentrepo.GetAllStudents();
        }
    }
}