using System;
using Lab2.Classes;
using Lab2.Extensions;
using Lab2.Generics;

namespace Lab2
{
    class Program
    {
        // 19. В функции Main демонстрируется использование всех элементов
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Static elements
            Console.WriteLine($"Welcome to {Station.StationName}");

            // Constructors & Composition
            Station station = new Station();

            // Indexer
            Console.WriteLine($"Checking platform 1: {station[0]}");

            // Abstract class, Inheritance, Encapsulation, Properties logic
            Train train1 = new Train("T-101", 120, 10);
            PassengerTrain pTrain = new PassengerTrain("P-202", 200, 8, true);

            // Extension method
            Console.WriteLine($"Is T-101 high speed? {train1.IsHighSpeed()}");
            Console.WriteLine($"Is P-202 high speed? {pTrain.IsHighSpeed()}");

            // Aggregation
            station.Arrive(train1);
            station.Arrive(pTrain);
            station.ShowTrains();

            // Overloaded operators
            Ticket ticket = new Ticket("Moscow", 50.0m);
            Console.WriteLine($"Original ticket: {ticket}");
            ticket = ticket + 10.5m;
            Console.WriteLine($"Ticket with fee: {ticket}");

            // Generics & Inheritance of generics
            LuggageStorage<string> luggage = new LuggageStorage<string>();
            luggage.Add("Suitcase #1");
            luggage.Add("Backpack #2");
            luggage.DisplayAll();

            // Generic method
            station.Announce<string>("Keep an eye on your belongings.");
            station.Announce<int>(5);

            Console.ReadKey();
        }
    }
}