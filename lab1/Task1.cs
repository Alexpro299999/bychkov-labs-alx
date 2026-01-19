using System;

namespace LabWork1
{
    public static class Task1
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("\n--- Задание 1: Вычисление функций ---");

                double xn = InputHelper.ReadDouble("Введите xn: ");

                double xk = InputHelper.ReadDouble(
                    "Введите xk (должно быть >= xn): ",
                    val => val >= xn,
                    "Ошибка: xk не может быть меньше xn."
                );

                double h = InputHelper.ReadDouble(
                    "Введите шаг h (h > 0): ",
                    val => val > 0,
                    "Ошибка: шаг h должен быть строго больше 0."
                );

                double yn = InputHelper.ReadDouble("Введите yn: ");

                double yk = InputHelper.ReadDouble(
                    "Введите yk (должно быть >= yn): ",
                    val => val >= yn,
                    "Ошибка: yk не может быть меньше yn."
                );

                double t = InputHelper.ReadDouble(
                    "Введите шаг t (t > 0): ",
                    val => val > 0,
                    "Ошибка: шаг t должен быть строго больше 0."
                );

                OutputHelper.PrintTableHeader();

                double y = yn;
                for (double x = xn; x <= xk + h / 10.0; x += h)
                {
                    if (y > yk + t / 10.0) y = yn;

                    string fRes, gRes;

                    try
                    {
                        fRes = CalculateF(x, y).ToString("F3");
                    }
                    catch
                    {
                        fRes = "не опр.";
                    }

                    try
                    {
                        gRes = CalculateG(x, y).ToString("F3");
                    }
                    catch
                    {
                        gRes = "не опр.";
                    }

                    OutputHelper.PrintTableRow(x, y, fRes, gRes);
                    y += t;
                }
                OutputHelper.PrintTableLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static double CalculateF(double x, double y)
        {
            double xy = x * y;
            double absXY = Math.Abs(xy);

            if (absXY < 2)
            {
                double denom = 2 + xy;
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException();
                return (Math.Pow(x - y, 2) + xy) / denom;
            }
            else if (absXY > 2)
            {
                double denomSq = Math.Pow(3 * x - 2, 2) + Math.Pow(y, 2);
                if (denomSq < 0) throw new ArithmeticException();
                double denom = Math.Sqrt(denomSq);
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException();
                return (Math.Pow(x + y, 3) + 2 * (y - x)) / denom;
            }
            else
            {
                return Math.Abs(Math.Pow(x, 2) + y) + 15;
            }
        }

        private static double CalculateG(double x, double y)
        {
            double xy = x * y;
            double absXY = Math.Abs(xy);

            if (absXY < 2)
            {
                double denom = 1 + x + y;
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException();
                return 5 / denom;
            }
            else if (absXY > 2)
            {
                return Math.Pow(y, 3) + x;
            }
            else
            {
                double denom = 1 + Math.Pow(x, 2);
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException();
                return ((Math.Pow(x, 2) + 6 * y) / denom) - 5 * y;
            }
        }
    }
}