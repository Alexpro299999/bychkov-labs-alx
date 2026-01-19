using System;

namespace LabWork1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.WriteLine("\n--- Главное меню ---");
                Console.WriteLine("1. Задание 1: Вычисление функций");
                Console.WriteLine("2. Задание 2: Векторы и Матрицы");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Task1.Run();
                        break;
                    case "2":
                        Task2.Run();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }
            }
        }
    }
}