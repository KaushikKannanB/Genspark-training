using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StudentMonitoringSOLID.Models;
using StudentMonitoringSOLID.Interfaces;

namespace StudentMonitoringSOLID.Repositories
{
    public class StudentRepositor : IStudentRepository
    {
        private readonly List<Student> students = new List<Student>();

        public int AddStudent(Student student)
        {
            students.Add(student);
            return student.Id;
        }

        public void UpdateStudent(Student student)
        {
            var existing = students.FirstOrDefault(s => s.Id == student.Id);
            if (existing != null)
            {
                existing.Name = student.Name;
                existing.department = student.department;
                existing.cgpa = student.cgpa;
            }
        }

        public Student GetStudentById(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        public List<Student> GetAllStudents()
        {
            return students;
        }
    }
}
