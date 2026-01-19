using System;
using System.Linq;

namespace Lab3
{
    public delegate bool ArrayCondition(int value);

    public static class Task2
    {
        public static void Run()
        {
            Console.WriteLine("\n--- Task 2: Vectors & Lambdas (Variant 3) ---");

            try
            {
                int n = InputHelper.ReadInt("Enter vector size n: ", v => v > 0, "Size must be > 0");
                int[] vector = InputHelper.ReadVector("Enter elements:", n);

                Console.WriteLine("Choose operation:");
                Console.WriteLine("1. Replace elements > 7 with 7 (and count)");
                Console.WriteLine("2. Calculate sum of negative elements");
                Console.Write("Selection: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    ProcessVector(vector, x => x > 7);
                }
                else if (choice == "2")
                {
                    CalculateSum(vector, x => x < 0);
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessVector(int[] array, ArrayCondition condition)
        {
            int[] tempArray = (int[])array.Clone();
            int count = 0;

            for (int i = 0; i < tempArray.Length; i++)
            {
                if (condition(tempArray[i]))
                {
                    tempArray[i] = 7;
                    count++;
                }
            }

            Console.WriteLine($"Modified vector: {string.Join(" ", tempArray)}");
            Console.WriteLine($"Count of replacements: {count}");
        }

        private static void CalculateSum(int[] array, ArrayCondition condition)
        {
            long sum = 0;
            bool found = false;

            foreach (var item in array)
            {
                if (condition(item))
                {
                    sum += item;
                    found = true;
                }
            }

            if (found)
                Console.WriteLine($"Sum of elements matching condition: {sum}");
            else
                Console.WriteLine("No elements match the condition.");
        }
    }
}