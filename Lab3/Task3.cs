using System;
using System.Linq;

namespace Lab3
{
    public delegate void VectorActionHandler(int[] vector, Func<int, bool> predicate);

    public class VectorProcessor
    {
        public event VectorActionHandler? OnProcess;

        public void Process(int[] vector, Func<int, bool> predicate)
        {
            if (OnProcess != null)
            {
                OnProcess(vector, predicate);
            }
            else
            {
                Console.WriteLine("No handlers subscribed.");
            }
        }
    }

    public static class Task3
    {
        public static void Run()
        {
            Console.WriteLine("\n--- Task 3: Events & Custom Exceptions ---");
            VectorProcessor processor = new VectorProcessor();

            try
            {
                int n = GetSizeWithValidation();
                int[] vector = InputHelper.ReadVector("Enter elements:", n);

                Console.WriteLine("Choose operation (via Event):");
                Console.WriteLine("1. Replace elements > 7 with 7");
                Console.WriteLine("2. Sum of negative elements");
                Console.Write("Selection: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    processor.OnProcess += ReplaceHandler;
                    processor.Process(vector, x => x > 7);
                }
                else if (choice == "2")
                {
                    processor.OnProcess += SumHandler;
                    processor.Process(vector, x => x < 0);
                }
                else
                {
                    throw new InvalidInputException("Invalid menu selection.");
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"[Custom Exception] {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Format Error: Invalid number format.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Task 3 execution finished.");
            }
        }

        private static int GetSizeWithValidation()
        {
            Console.Write("Enter vector size n: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int n))
            {
                throw new InvalidInputException("Size must be an integer.");
            }

            if (n <= 0)
            {
                throw new InvalidInputException("Size must be greater than zero.");
            }

            return n;
        }

        private static void ReplaceHandler(int[] vector, Func<int, bool> condition)
        {
            Console.WriteLine("-> ReplaceHandler invoked.");
            int[] temp = (int[])vector.Clone();
            int count = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (condition(temp[i]))
                {
                    temp[i] = 7;
                    count++;
                }
            }
            Console.WriteLine($"Result: {string.Join(" ", temp)} (Replacements: {count})");
        }

        private static void SumHandler(int[] vector, Func<int, bool> condition)
        {
            Console.WriteLine("-> SumHandler invoked.");
            long sum = vector.Where(condition).Sum(x => (long)x);
            Console.WriteLine($"Result Sum: {sum}");
        }
    }
}