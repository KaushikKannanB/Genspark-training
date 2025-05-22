using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StudentMonitoringSOLID.Models;

namespace StudentMonitoringSOLID.Interfaces
{
    public interface IStudentRepository
    {
        int AddStudent(Student student);
        void UpdateStudent(Student student);
        Student GetStudentById(int id);
        List<Student> GetAllStudents();
    }
}