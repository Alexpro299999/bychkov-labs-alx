using System;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LinqDemo.Run();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}