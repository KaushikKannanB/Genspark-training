using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardiologist.Interfaces;
using Cardiologist.Models;

namespace Cardiologist
{
    public class ManageAppointments
    {
        private readonly IPatientService PatientService;

        public ManageAppointments(IPatientService patientservice)
        {
            PatientService = patientservice;
        }
        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                int option = 0;
                while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 && option > 2))
                {
                    Console.WriteLine("Invalid entry. Please enter a valid option");
                }
                switch (option)
                {
                    case 1:
                        AddAppointments();
                        break;
                    case 2:
                        SearchAppointment();
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
        }
        public void PrintMenu()
        {
            Console.WriteLine("Choose what you wanted");
            Console.WriteLine("1. Add Appointment");
            Console.WriteLine("2. Search Appointment");
        }
        public void AddAppointments()
        {
            Appointment app = new Appointment();
            app.TakeInputFromUser();
            int id = PatientService.AddAppointment(app);
            Console.WriteLine($"The employee added. The Id is {id}");
        }
        public void SearchAppointment()
        {
            var searchMenu = PrintSearchMenu();
            var apps = PatientService.SearchAppointment(searchMenu);
            Console.WriteLine("The search options you have selected");
            Console.WriteLine(searchMenu);
            if (apps == null)
            {
                Console.WriteLine("No Appointments for the search");
            }
            PrintAppointments(apps);

        }

        private void PrintAppointments(List<Appointment>?apps)
        {
            if (apps == null)
            {
                Console.WriteLine("No such appointments exist");
                return;
            }
            foreach (var a in apps)
                {
                    Console.WriteLine(a);
                }
        }

        private SearchModel PrintSearchMenu()
        {
            Console.WriteLine("Please select the search option");
            SearchModel searchModel = new SearchModel();
            int idOption;
            Console.WriteLine("Search by Name. ? Type 1 for yes Type 2 no");
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (idOption == 1)
            {
                Console.WriteLine("Please enter the Patient Name");
                string name = Console.ReadLine() ?? "";
                searchModel.Name = name;
                idOption = 0;
            }
            Console.WriteLine("3. Search by Age. Please enter 1 for yes and 2 for no");
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if(idOption == 1)
            {
                searchModel.Age = new Range<int>();
                int age;
                Console.WriteLine("Please enter the min Patient Age");
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
                {
                    Console.WriteLine("Invalid entry for min age. Please enter a valid age");
                }
                searchModel.Age.MinVal = age;
                Console.WriteLine("Please enter the max patient Age");
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
                {
                    Console.WriteLine("Invalid entry for max age. Please enter a valid age");
                }
                searchModel.Age.MaxVal = age;
                idOption = 0;
            }

            Console.WriteLine("4. Search by Appointment Date. Please enter 1 for yes and 2 for no");
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if(idOption == 1)
            {
                DateTime date;
                Console.WriteLine("Please enter the date");
                while (!DateTime.TryParse(Console.ReadLine(), out date))
                {
                    Console.WriteLine("Invalid entry for date");
                }
                searchModel.appointmentdate = date;
                Console.WriteLine("Please enter the max employee Salary");
                idOption = 0;
            }
            return searchModel;
        }
    }
}