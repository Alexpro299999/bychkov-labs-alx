using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            // source_text.txt and result_text.txt in bin directory (Lab4\bin\Debug\net8.0\) for Task 2. cars.json is created for Task 3. Cars folder is created on Desktop.

            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Strings (Duplicate words)");
                Console.WriteLine("2. Text File (Reverse words)");
                Console.WriteLine("3. JSON File (Cars Database)");
                Console.WriteLine("4. Exit");
                Console.Write("Select option: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RunStringTask();
                        break;
                    case "2":
                        RunFileTask();
                        break;
                    case "3":
                        RunJsonTask();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }

        static void RunStringTask()
        {
            Console.WriteLine("\nEnter text:");
            string? text = Console.ReadLine();
            if (text != null)
            {
                string result = StringProcessor.DuplicateSpecialWords(text);
                Console.WriteLine("Result:");
                Console.WriteLine(result);
            }
        }

        static void RunFileTask()
        {
            string inputFile = "source_text.txt";
            string outputFile = "result_text.txt";

            if (!File.Exists(inputFile))
            {
                File.WriteAllLines(inputFile, new[]
                {
                    "Hello world this is a test",
                    "One Two Three",
                    "Coding is fun"
                });
                Console.WriteLine($"File {inputFile} created with sample data.");
            }

            string[] lines = File.ReadAllLines(inputFile);
            var processedLines = new List<string>();

            foreach (var line in lines)
            {
                processedLines.Add(StringProcessor.ReverseWordsInLine(line));
            }

            File.WriteAllLines(outputFile, processedLines);
            Console.WriteLine($"Processed data written to {outputFile}");
            Console.WriteLine("Result file content:");
            foreach (var line in processedLines)
            {
                Console.WriteLine(line);
            }
        }

        static void RunJsonTask()
        {
            string fileName = "cars.json";

            var cars = new List<Car>
            {
                new Car { NameOwner = "Ivan", ModelCar = "Toyota", NumberCar = "A123AA" },
                new Car { NameOwner = "Petr", ModelCar = "BMW", NumberCar = "B456BB" },
                new Car { NameOwner = "Maria", ModelCar = "Toyota", NumberCar = "C789CC" },
                new Car { NameOwner = "Alex", ModelCar = "Lada", NumberCar = "E001KX" },
                new Car { NameOwner = "Olga", ModelCar = "BMW", NumberCar = "H111HH" }
            };

            string jsonString = JsonSerializer.Serialize(cars, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine($"JSON file {fileName} created and populated.");

            string readJson = File.ReadAllText(fileName);
            List<Car>? loadedCars = JsonSerializer.Deserialize<List<Car>>(readJson);

            if (loadedCars != null)
            {
                var groupedData = JsonProcessor.GroupCarsByModel(loadedCars);

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string carsDirectory = Path.Combine(desktopPath, "Cars");

                if (!Directory.Exists(carsDirectory))
                {
                    Directory.CreateDirectory(carsDirectory);
                }

                foreach (var modelGroup in groupedData)
                {
                    string modelFile = Path.Combine(carsDirectory, $"{modelGroup.Key}.txt");
                    File.WriteAllLines(modelFile, modelGroup.Value);
                    Console.WriteLine($"Created file: {modelFile}");
                }

                Console.WriteLine($"Operation complete. Check 'Cars' folder on Desktop.");
            }
        }
    }
}