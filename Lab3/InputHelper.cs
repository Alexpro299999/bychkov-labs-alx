using System;
using System.Linq;

namespace Lab3
{
    public static class InputHelper
    {
        public static double ReadDouble(string message, Func<double, bool>? validator = null, string? errorMessage = null)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (double.TryParse(input, out double result))
                {
                    if (double.IsNaN(result) || double.IsInfinity(result))
                    {
                        Console.WriteLine("Error: Invalid number. Try again.");
                        continue;
                    }

                    if (validator == null || validator(result))
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine(errorMessage ?? "Error: Value does not meet the condition.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Not a number. Try again.");
                }
            }
        }

        public static int ReadInt(string message, Func<int, bool>? validator = null, string? errorMessage = null)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (validator == null || validator(result))
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine(errorMessage ?? "Error: Value does not meet the condition.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Not an integer.");
                }
            }
        }

        public static int[] ReadVector(string message, int size)
        {
            Console.WriteLine(message);
            while (true)
            {
                try
                {
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Error: Empty input.");
                        continue;
                    }

                    int[] vector = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(int.Parse)
                                           .ToArray();

                    if (vector.Length != size)
                    {
                        Console.WriteLine($"Error: Expected {size} elements, got {vector.Length}. Try again.");
                        continue;
                    }
                    return vector;
                }
                catch
                {
                    Console.WriteLine("Error: Invalid format. Enter integers separated by space.");
                }
            }
        }
    }
}