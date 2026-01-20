using System;
using System.Threading.Tasks;

namespace Lab9
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n--- Лабораторная работа 9 ---");
                Console.WriteLine("1. Задание 1: Дикари и Повар (Семафоры)");
                Console.WriteLine("2. Задание 2: Async/Await и ManualResetEvent");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        int n = GetValidInput("Введите количество дикарей (n)");
                        int m = GetValidInput("Введите вместимость горшка (m)");
                        DiningSavages.Run(n, m);
                        break;
                    case "2":
                        await AsyncAndSyncDemo.Run();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        static int GetValidInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write($"{prompt}: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out value) && value > 0)
                {
                    return value;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка! Введите целое число больше 0 (не дробное, не букву).");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}