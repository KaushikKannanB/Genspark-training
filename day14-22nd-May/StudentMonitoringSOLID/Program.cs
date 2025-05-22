using StudentMonitoringSOLID.Interfaces;
using StudentMonitoringSOLID.Repositories;
using StudentMonitoringSOLID.Services;
using StudentMonitoringSOLID.UI;

class Program
{
    static void Main(string[] args)
    {
        IStudentRepository repository = new StudentRepositor();
        StudentService service = new StudentService(repository);
        StudentConsoleUI ui = new StudentConsoleUI(service);
        ui.Run();
    }
}
