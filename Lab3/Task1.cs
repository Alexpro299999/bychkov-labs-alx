using System;

namespace Lab3
{
    public delegate double MathFunction(double x, double y);

    public static class Task1
    {
        public static void Run()
        {
            Console.WriteLine("\n--- Task 1: Delegates (Variant 3) ---");

            try
            {
                double x = InputHelper.ReadDouble("Enter x: ");
                double y = InputHelper.ReadDouble("Enter y: ");

                Console.WriteLine("Select function to calculate:");
                Console.WriteLine("1. Function f(x, y)");
                Console.WriteLine("2. Function g(x, y)");
                Console.Write("Selection: ");

                string? choice = Console.ReadLine();
                MathFunction selectedFunc;

                if (choice == "1")
                {
                    selectedFunc = CalculateF;
                    Console.WriteLine("Selected function: f");
                }
                else if (choice == "2")
                {
                    selectedFunc = CalculateG;
                    Console.WriteLine("Selected function: g");
                }
                else
                {
                    Console.WriteLine("Invalid selection. Defaulting to f.");
                    selectedFunc = CalculateF;
                }

                CalculateZ(x, y, selectedFunc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void CalculateZ(double x, double y, MathFunction func)
        {
            try
            {
                double result = func(x, y);
                Console.WriteLine($"Result z = {result:F3}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Calculation error: {ex.Message}");
            }
        }

        private static double CalculateF(double x, double y)
        {
            double xy = x * y;
            double absXY = Math.Abs(xy);

            if (absXY < 2)
            {
                double denom = 2 + xy;
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException("Denominator is zero in f.");
                return (Math.Pow(x - y, 2) + xy) / denom;
            }
            else if (absXY > 2)
            {
                double denomSq = Math.Pow(3 * x - 2, 2) + Math.Pow(y, 2);
                double denom = Math.Sqrt(denomSq);
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException("Denominator is zero in f.");
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
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException("Denominator is zero in g.");
                return 5 / denom;
            }
            else if (absXY > 2)
            {
                return Math.Pow(y, 3) + x;
            }
            else
            {
                double denom = 1 + Math.Pow(x, 2);
                if (Math.Abs(denom) < 1e-9) throw new DivideByZeroException("Denominator is zero in g.");
                return ((Math.Pow(x, 2) + 6 * y) / denom) - 5 * y;
            }
        }
    }
}