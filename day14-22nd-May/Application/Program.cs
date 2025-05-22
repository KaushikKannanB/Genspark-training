using System;
class Program
{
    public delegate void Mydelegate(int n1, int n2);

    public static void Add(int a, int b)
    {
        Console.WriteLine($"{a + b}");
    }

    public static void Mul(int a, int b)
    {
        Console.WriteLine($"{a * b}");
    }

    public static void Main(string [] args)
    {
        // Mydelegate del = new Mydelegate(Add);

        // syntax: Action<T,T> delegatename;
        Action<int, int> del = Add;
        del += Mul;

        del += (int n1, int n2) => Console.WriteLine($"{n1 / n2}");
        del(6, 3);
    }
}
