using System;
using System.Linq;

namespace LabWork1
{
    public static class InputHelper
    {
        public static double ReadDouble(string message, Func<double, bool> validator = null, string errorMessage = null)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (double.TryParse(input, out double result))
                {
                    if (double.IsNaN(result) || double.IsInfinity(result))
                    {
                        Console.WriteLine("Ошибка: введено некорректное число. Попробуйте снова.");
                        continue;
                    }

                    if (validator == null || validator(result))
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine(errorMessage ?? "Ошибка: значение не удовлетворяет условию.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: введено не число. Попробуйте снова.");
                }
            }
        }

        public static int ReadInt(string message, Func<int, bool> validator = null, string errorMessage = null)
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
                        Console.WriteLine(errorMessage ?? "Ошибка: значение не удовлетворяет условию.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: введено не целое число.");
                }
            }
        }

        public static double[] ReadVector(string message, int size)
        {
            Console.WriteLine(message);
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: пустой ввод.");
                        continue;
                    }

                    double[] vector = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(double.Parse)
                                           .ToArray();

                    if (vector.Length != size)
                    {
                        Console.WriteLine($"Ошибка: ожидалось {size} элементов, а введено {vector.Length}. Попробуйте снова.");
                        continue;
                    }
                    return vector;
                }
                catch
                {
                    Console.WriteLine("Ошибка формата. Вводите только числа через пробел.");
                }
            }
        }

        public static double[,] ReadSquareMatrix(string message)
        {
            int n = ReadInt(message, x => x > 0, "Размерность матрицы должна быть больше 0.");

            double[,] matrix = new double[n, n];
            Console.WriteLine($"Введите {n} строк по {n} элементов (через пробел):");

            for (int i = 0; i < n; i++)
            {
                double[] row = ReadVector($"Строка {i + 1}:", n);
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = row[j];
                }
            }
            return matrix;
        }
    }
}