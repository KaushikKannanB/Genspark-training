using System;
class EmployeePromotion
{
    public string name { get; set; }
    public static void ViewPromotionList(List<EmployeePromotion> promotionList)
    {
        foreach (EmployeePromotion emp in promotionList)
        {
            Console.WriteLine($"xxx {emp.name}");
        }
    }
    public static int ViewPosition(List<EmployeePromotion> promotionList, string employeename)
    {
        return promotionList.FindIndex(emp=> emp.name==employeename);
    }
}
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter employees in order of their upcoming promotion");
        List<EmployeePromotion> promotionList = new List<EmployeePromotion>();

        string Name;
        string posFound;
        while (true)
        {
            Name = Console.ReadLine();
            if (string.IsNullOrEmpty(Name))
            {
                break;
            }
            else
            {
                EmployeePromotion emp = new EmployeePromotion();
                emp.name = Name;
                promotionList.Add(emp);
            }
        }
        Console.WriteLine("Enter the Question Number:");
        int questionno = int.Parse(Console.ReadLine());

        switch (questionno)
        {
            case 1:
                EmployeePromotion.ViewPromotionList(promotionList);
                break;
            case 2:
                Console.WriteLine("Position to be found for employee below:");
                posFound = Console.ReadLine();
                int ind = EmployeePromotion.ViewPosition(promotionList, posFound);
                if (ind == -1)
                {
                    Console.WriteLine("Not found!");
                }
                else
                    Console.WriteLine($"{posFound}'s position in the promotion list is {ind + 1}");
                break;
            case 3:
                Console.WriteLine($"Capacity of the List: {promotionList.Capacity}");
                Console.WriteLine($"Current size of the List: {promotionList.Count}");
                promotionList.TrimExcess(); // trims only if count<capacity*0.9 and if meaningful differences...
                Console.WriteLine($"Capacity of the List - after TRIMMING: {promotionList.Capacity}");
                break;
            case 4:
                List<EmployeePromotion> sortedlist = promotionList.OrderBy(emp => emp.name).ToList();
                EmployeePromotion.ViewPromotionList(sortedlist);
                break;
        }
    }
}