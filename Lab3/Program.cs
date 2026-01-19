using System;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.WriteLine("\n--- Lab 3 Main Menu ---");
                Console.WriteLine("1. Task 1: Function Delegate (f/g)");
                Console.WriteLine("2. Task 2: Vector Delegate (Action/Func)");
                Console.WriteLine("3. Task 3: Vector Events & Exceptions");
                Console.WriteLine("4. Exit");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Task1.Run();
                        break;
                    case "2":
                        Task2.Run();
                        break;
                    case "3":
                        Task3.Run();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }
    }
}