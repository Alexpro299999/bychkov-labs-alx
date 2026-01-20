using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab9
{
    public static class AsyncAndSyncDemo
    {
        private static ManualResetEvent _mre = new ManualResetEvent(false);

        public static async Task Run()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Задание 2.1: Async / Await ---");

            Console.WriteLine("Main: Запускаем долгую операцию...");
            Task<int> task = LongOperationAsync();

            Console.WriteLine("Main: Пока операция идет, мы можем делать что-то еще.");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Main: работа...");
                Thread.Sleep(300);
            }

            int result = await task;
            Console.WriteLine($"Main: Результат получен: {result}");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- Задание 2.2 (в): ManualResetEvent ---");
            Console.WriteLine("Ситуация: Несколько потоков ждут открытия 'ворот'.");

            _mre.Reset();

            for (int i = 1; i <= 5; i++)
            {
                Thread t = new Thread(Worker);
                t.Start(i);
            }

            Console.WriteLine("Главный поток: Ворота закрыты. Ждем 2 секунды...");
            Thread.Sleep(2000);

            Console.WriteLine("Главный поток: ОТКРЫВАЮ ВОРОТА! (_mre.Set())");
            _mre.Set();

            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static async Task<int> LongOperationAsync()
        {
            Console.WriteLine("Async: Начало вычислений...");
            await Task.Delay(2000);
            Console.WriteLine("Async: Вычисления завершены.");
            return 42;
        }

        private static void Worker(object id)
        {
            Console.WriteLine($"Поток {id} подошел к воротам и ждет.");
            _mre.WaitOne();
            Console.WriteLine($"Поток {id} прошел сквозь ворота!");
        }
    }
}