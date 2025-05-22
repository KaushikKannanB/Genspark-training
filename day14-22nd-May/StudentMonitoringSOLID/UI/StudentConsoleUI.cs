using StudentMonitoringSOLID.Models;
using StudentMonitoringSOLID.Services;

namespace StudentMonitoringSOLID.UI
{
    public class StudentConsoleUI
    {
        private readonly StudentService _studentService;

        public StudentConsoleUI(StudentService studentService)
        {
            _studentService = studentService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Student Monitoring System ===");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Update Student");
                Console.WriteLine("3. View All Students");
                Console.WriteLine("4. View Student By ID");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var newStudent = TakeInputFromUser();
                        Console.WriteLine(_studentService.AddStudent(newStudent));
                        break;

                    case "2":
                        Console.WriteLine("Enter ID of student to update:");
                        int id;
                        while (!int.TryParse(Console.ReadLine(), out id))
                            Console.WriteLine("Enter a valid number");

                        var existing = _studentService.GetStudentById(id);
                        if (existing == null)
                        {
                            Console.WriteLine("Student not found.");
                        }
                        else
                        {
                            Console.WriteLine("Enter updated details:");
                            var updatedStudent = TakeInputFromUser();
                            updatedStudent.Id = id;
                            _studentService.UpdateStudent(updatedStudent);
                        }
                        break;

                    case "3":
                        var all = _studentService.GetAllStudents();
                        if (all.Count == 0)
                        {
                            Console.WriteLine("No students found.");
                        }
                        else
                        {
                            foreach (var s in all)
                            {
                                Console.WriteLine("\n-----------------");
                                Console.WriteLine(s);
                            }
                        }
                        break;

                    case "4":
                        Console.Write("Enter student ID: ");
                        if (int.TryParse(Console.ReadLine(), out int studentId))
                        {
                            var student = _studentService.GetStudentById(studentId);
                            if (student != null)
                                Console.WriteLine(student);
                            else
                                Console.WriteLine("Student not found.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;

                    case "5":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        public static Student TakeInputFromUser()
        {
            Console.WriteLine("Enter Student ID:");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Enter a valid number as ID");
            }

            Console.WriteLine("Enter Student Name:");
            string name = Console.ReadLine() ?? "";

            Console.WriteLine("Enter Student Department:");
            string dept = Console.ReadLine() ?? "";

            Console.WriteLine("Enter Student CGPA:");
            double cgpa;
            while (!double.TryParse(Console.ReadLine(), out cgpa))
            {
                Console.WriteLine("Enter a valid value as CGPA");
            }

            return new Student(id, name, dept, cgpa);
        }
    }
}
