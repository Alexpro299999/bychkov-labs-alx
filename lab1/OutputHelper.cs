using System;
using System.Linq;

namespace LabWork1
{
    public static class OutputHelper
    {
        public static void PrintTableLine()
        {
            Console.WriteLine(new string('-', 49));
        }

        public static void PrintTableHeader()
        {
            PrintTableLine();
            Console.WriteLine("| {0,-10} | {1,-10} | {2,-10} | {3,-10} |", "x", "y", "f", "g");
            PrintTableLine();
        }

        public static void PrintTableRow(double x, double y, string f, string g)
        {
            Console.WriteLine("| {0,-10:F2} | {1,-10:F2} | {2,-10} | {3,-10} |", x, y, f, g);
        }

        public static void PrintVector(double[] vector)
        {
            Console.WriteLine(string.Join(" ", vector.Select(v => v.ToString("F2"))));
        }

        public static void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],-8:F2}");
                }
                Console.WriteLine();
            }
        }
    }
}