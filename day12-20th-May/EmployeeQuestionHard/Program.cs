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
    public static Dictionary<int, Employee> ModifyEmployee(Dictionary<int, Employee> employees)
    {
        Console.WriteLine("Enter the Employee ID to Modify:");
        int id = Check();

        if (!employees.ContainsKey(id))
        {
            Console.WriteLine("No such employee exists");
            return employees;
        }

        Employee emp = employees[id];
        Console.WriteLine($"Current Data - Name: {emp.name}, Age: {emp.age}, Salary: {emp.salary}");

        Console.WriteLine("Enter new Name:");
        emp.name = StringCheck();
        Console.WriteLine("Enter new Age:");
        emp.age = Check();
        Console.WriteLine("Enter new Salary:");
        emp.salary = double.Parse(Console.ReadLine());

        employees[id] = emp;
        Console.WriteLine("Employee updated successfully.");

        return employees;
    }

    public static Dictionary<int, Employee> DeleteEmployee(Dictionary<int, Employee> employees)
    {
        Console.WriteLine("Enter the Employee ID to Delete:");
        int id = Check();

        if (employees.Remove(id))
        {
            Console.WriteLine("Employee deleted successfully.");
        }
        else
        {
            Console.WriteLine("No such employee exists.");
        }

        return employees;
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
        bool isrunning = true;
        Console.WriteLine("Welcome to Employee management site");
        while (isrunning)
        {
            Console.WriteLine("Enter 1: View, 2: Insert, 3: Find by ID, 4: Update, 5: Delete, others: Quit");
            int questionno = int.Parse(Console.ReadLine());
            switch (questionno)
            {
                case 1:
                    Console.WriteLine("\nEmployee Database:");
                    Employee.ViewEmployeeDB(employees);
                    break;
                case 2:
                    employees = Employee.InsertEmployee(employees);
                    break;
                case 3:
                    Console.WriteLine("Employee Find ID:");
                    {
                        int idToCheck = Employee.Check();
                        Employee.FindEmployee(employees, idToCheck);
                    }
                    break;
                case 4:
                    Console.WriteLine("\nModify Employee:");
                    {
                        employees = Employee.ModifyEmployee(employees);
                        Employee.ViewEmployeeDB(employees);
                        break;
                    }

                case 5:
                    Console.WriteLine("\nDelete Employee:");
                    {
                        employees = Employee.DeleteEmployee(employees);
                        Employee.ViewEmployeeDB(employees);
                        break;
                    }
                default:
                    isrunning = false;
                    break;
            }
        }
    }
}
