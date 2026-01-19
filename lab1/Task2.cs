using System;

namespace LabWork1
{
    public static class Task2
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Задание 2: Меню ---");
                Console.WriteLine("1. Вектор: Замена > 7, подсчет по четности индекса");
                Console.WriteLine("2. Матрица: Вектор b = сумма отрицательных элементов строк");
                Console.WriteLine("3. Матрица: Максимальный элемент ниже главной диагонали");
                Console.WriteLine("4. Назад");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SubTask1();
                        break;
                    case "2":
                        SubTask2();
                        break;
                    case "3":
                        SubTask3();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный пункт.");
                        break;
                }
            }
        }

        private static void SubTask1()
        {
            int n = InputHelper.ReadInt("Введите размер вектора n: ", val => val > 0, "Размер должен быть больше 0.");

            double[] arr = InputHelper.ReadVector("Введите элементы:", n);

            Console.WriteLine("Исходный вектор:");
            OutputHelper.PrintVector(arr);

            ProcessVector(arr, out int evenIdxCount, out int oddIdxCount);

            Console.WriteLine("Измененный вектор:");
            OutputHelper.PrintVector(arr);
            Console.WriteLine($"Замен на четных индексах: {evenIdxCount}");
            Console.WriteLine($"Замен на нечетных индексах: {oddIdxCount}");
        }

        private static void SubTask2()
        {
            try
            {
                double[,] matrix = InputHelper.ReadSquareMatrix("Введите размер матрицы n: ");

                Console.WriteLine("Исходная матрица:");
                OutputHelper.PrintMatrix(matrix);

                double[] b = GetRowNegSums(matrix);

                Console.WriteLine("Вектор b (сумма отрицательных в строках):");
                OutputHelper.PrintVector(b);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SubTask3()
        {
            try
            {
                double[,] matrix = InputHelper.ReadSquareMatrix("Введите размер матрицы n: ");

                Console.WriteLine("Исходная матрица:");
                OutputHelper.PrintMatrix(matrix);

                if (GetMaxBelowDiagonal(matrix, out double maxVal))
                {
                    Console.WriteLine($"Макс. элемент ниже главной диагонали: {maxVal:F2}");
                }
                else
                {
                    Console.WriteLine("Нет элементов ниже главной диагонали.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ProcessVector(double[] arr, out int evenCount, out int oddCount)
        {
            evenCount = 0;
            oddCount = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > 7)
                {
                    arr[i] = 7;
                    if (i % 2 == 0) evenCount++;
                    else oddCount++;
                }
            }
        }

        private static double[] GetRowNegSums(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[] b = new double[n];

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] < 0)
                    {
                        sum += matrix[i, j];
                    }
                }
                b[i] = sum;
            }
            return b;
        }

        private static bool GetMaxBelowDiagonal(double[,] matrix, out double maxVal)
        {
            int n = matrix.GetLength(0);
            maxVal = double.MinValue;
            bool found = false;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (!found || matrix[i, j] > maxVal)
                    {
                        maxVal = matrix[i, j];
                        found = true;
                    }
                }
            }
            return found;
        }
    }
}