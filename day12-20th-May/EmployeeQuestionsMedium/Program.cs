using System;
using System.Collections.Generic;

class Employee
{
    public int id { get; set; }
    public string name { get; set; }
    public int age { get; set; }
    public double salary { get; set; }

    public static void ViewEmployeeDB(Dictionary<int, Employee> emp)
    {
        foreach (var e in emp)
        {
            Console.WriteLine($"EmpId: {e.Value.id} - EmpName: {e.Value.name} - EmpAge: {e.Value.age} - EmpSalary: {e.Value.salary}");
        }
    }

    public static Dictionary<int, Employee> InsertEmployee(Dictionary<int, Employee> employees)
    {
        Console.WriteLine("Enter the employee details");
        Console.WriteLine("Enter the employee Id");

        int id = Check();

        if (employees.ContainsKey(id))
        {
            Console.WriteLine("Id Already exists, Please enter another Id");
        }
        else
        {
            Employee e = new Employee();
            e.id = id;
            Console.WriteLine("Enter the Name");
            e.name = StringCheck();
            Console.WriteLine("Enter the Age");
            e.age = Check();
            Console.WriteLine("Enter the salary");

            e.salary = double.Parse(Console.ReadLine());

            employees[id] = e;
        }

        return employees;
    }

    public static void FindEmployee(Dictionary<int, Employee> emp, int id)
    {
        var e = emp.FirstOrDefault(e => e.Key == id);
        if (e.Equals(default(KeyValuePair<int, Employee>)))
        {
            Console.WriteLine("No such employee exists");
        }
        else
        {
            Employee em = e.Value;
            Console.WriteLine($"EmpId: {em.id} - EmpName: {em.name} - EmpAge: {em.age} - EmpSalary: {em.salary}");

        }

    }

    public static void FindEmployeeName(Dictionary<int, Employee> emp, string name)
    {
        var e = emp.Where(e => e.Value.name == name).ToList();
        if (e.Count == 0)
        {
            Console.WriteLine("No such employee exists");

        }
        else
        {
            foreach (var i in e)
            {
                Employee item = i.Value;
                Console.WriteLine($"EmpId: {item.id} - EmpName: {item.name} - EmpAge: {item.age} - EmpSalary: {item.salary}");
            }
        }
    }
    public static void FindElder(Dictionary<int, Employee> emp, int id)
    {
        var e = emp.FirstOrDefault(e => e.Key == id);
        if (e.Equals(default(KeyValuePair<int, Employee>)))
        {
            Console.WriteLine("No such employee exists");
        }
        else
        {
            int age_to_compare = e.Value.age;
            var older = emp.Where(e => e.Value.age > age_to_compare).ToList();
            if (older == null)
            {
                Console.WriteLine($"No one is older than {id}");
            }
            foreach (var i in older)
                {
                    Employee item = i.Value;
                    Console.WriteLine($"EmpId: {item.id} - EmpName: {item.name} - EmpAge: {item.age} - EmpSalary: {item.salary}");
                }
        }
    }
    public static List<Employee> SortBySalary(Dictionary<int, Employee> emp)
    {
        List<Employee> emplist = new List<Employee>();
        foreach (var e in emp)
        {
            emplist.Add(e.Value);
        }

        return emplist.OrderBy(e => e.salary).ToList();
    }
    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Please enter a Valid Number");
        }
        return num;
    }
    
    public static string StringCheck()
    {
        string input = "";
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Enter a valid name:");
            input = Console.ReadLine();
        }
        return input;
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Dictionary<int, Employee> employees = new Dictionary<int, Employee>();
        List<Employee> emplist = new List<Employee>();

        Console.WriteLine("How many employees to insert?");
        int num = Employee.Check();

        for (int i = 0; i < num; i++)
        {
            employees = Employee.InsertEmployee(employees);
        }

        Console.WriteLine("\nEmployee Database:");
        Employee.ViewEmployeeDB(employees);
        Console.WriteLine("Enter the Question Number:");
        int questionno = int.Parse(Console.ReadLine());
        switch (questionno)
        {
            case 1:
                Console.WriteLine("Employee Find ID:");
                {
                    int idToCheck = Employee.Check();
                    Employee.FindEmployee(employees, idToCheck);
                }
                break;

            case 2:
                Console.WriteLine("\nEmployee Sort:");
                {
                    emplist = Employee.SortBySalary(employees);

                    foreach (Employee emp in emplist)
                    {
                        Console.WriteLine($"EmpId: {emp.id} - EmpName: {emp.name} - EmpAge: {emp.age} - EmpSalary: {emp.salary}");
                    }

                    Console.WriteLine("Employee Find ID:");
                    int idToCheck = Employee.Check();
                    Employee found = emplist.FirstOrDefault(emp => emp.id == idToCheck);

                    if (found == null)
                    {
                        Console.WriteLine("No such Employee");
                    }
                    else
                    {
                        Console.WriteLine($"EmpId: {found.id} - EmpName: {found.name} - EmpAge: {found.age} - EmpSalary: {found.salary}");
                    }
                }
                break;

            case 3:
                Console.WriteLine("\nEmployee Find Name:");
                {
                    string nameToFind = Employee.StringCheck(); // assuming it reads name from console
                    Employee.FindEmployeeName(employees, nameToFind);
                }
                break;

            case 4:
                Console.WriteLine("\nEmployee older:");
                {
                    int ageToCheck = Employee.Check();
                    Employee.FindElder(employees, ageToCheck); // assuming it finds employees older than the given age
                }
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

    }
}
