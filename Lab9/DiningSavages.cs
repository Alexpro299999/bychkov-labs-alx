using System;
using System.Threading;

namespace Lab9
{
    public static class DiningSavages
    {
        private static int _servings;
        private static int _potCapacity;
        private static int _savagesCount;
        private static bool _isRunning;

        private static Semaphore _mutex;
        private static Semaphore _emptyPot;
        private static Semaphore _fullPot;

        public static void Run(int n, int m)
        {
            _savagesCount = n;
            _potCapacity = m;
            _servings = m;
            _isRunning = true;

            _mutex = new Semaphore(1, 1);
            _emptyPot = new Semaphore(0, 1);
            _fullPot = new Semaphore(0, 1);

            Console.WriteLine($"--- Старт симуляции: {n} дикарей, {m} кусков ---");
            Console.WriteLine("Нажмите Enter, чтобы остановить симуляцию и вернуться в меню.");

            Thread cookThread = new Thread(CookLife);
            cookThread.IsBackground = true;
            cookThread.Start();

            for (int i = 0; i < _savagesCount; i++)
            {
                int id = i + 1;
                Thread t = new Thread(() => SavageLife(id));
                t.IsBackground = true;
                t.Start();
            }

            Console.ReadLine();

            _isRunning = false;

            try
            {
                if (_fullPot.WaitOne(0)) _fullPot.Release();
                if (_emptyPot.WaitOne(0)) _emptyPot.Release();
                if (_mutex.WaitOne(0)) _mutex.Release();
            }
            catch { }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Симуляция остановлена.");
        }

        private static void SavageLife(int id)
        {
            while (_isRunning)
            {
                _mutex.WaitOne();

                if (!_isRunning)
                {
                    _mutex.Release();
                    return;
                }

                if (_servings == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Дикарь {id} видит пустой горшок и будит повара!");
                    _emptyPot.Release();
                    _fullPot.WaitOne();
                }

                if (_servings > 0)
                {
                    _servings--;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Дикарь {id} ест. В горшке осталось: {_servings}");
                }

                _mutex.Release();

                Thread.Sleep(new Random().Next(100, 500));
            }
        }

        private static void CookLife()
        {
            while (_isRunning)
            {
                _emptyPot.WaitOne();

                if (!_isRunning) return;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Повар проснулся и готовит еду...");
                Thread.Sleep(1000);
                _servings = _potCapacity;
                Console.WriteLine($"Повар наполнил горшок ({_potCapacity} кусков) и пошел спать.");

                _fullPot.Release();
            }
        }
    }
}