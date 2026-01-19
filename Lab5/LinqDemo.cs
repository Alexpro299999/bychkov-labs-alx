using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    public static class LinqDemo
    {
        private static List<Passenger> _passengers = DataSeeder.GetPassengers();
        private static List<Flight> _flights = DataSeeder.GetFlights();

        public static void Run()
        {
            ShowData();

            Task1_FilterSimple();
            Task2_FilterTwoCriteria();
            Task3_Sort();
            Task4_Count();
            Task5_Aggregates();
            Task6_LetOperator();
            Task7_Grouping();
            Task8_Join();
            Task9_GroupJoin();
            Task10_All();
            Task11_Any();
        }

        private static void ShowData()
        {
            Console.WriteLine("--- PASSENGERS ---");
            foreach (var p in _passengers) Console.WriteLine(p);
            Console.WriteLine("\n--- FLIGHTS ---");
            foreach (var f in _flights) Console.WriteLine(f);
            Console.WriteLine();
        }

        private static void Task1_FilterSimple()
        {
            Console.WriteLine("1. Filter: Passengers with luggage > 20kg (Method Syntax)");
            var result = _passengers.Where(p => p.LuggageWeight > 20);
            foreach (var item in result) Console.WriteLine(item);
            Console.WriteLine();
        }

        private static void Task2_FilterTwoCriteria()
        {
            Console.WriteLine("2. Filter: Luggage < 20kg AND specific Flight (User Input) (Query Syntax)");
            Console.Write("Enter Flight Number (e.g. SU-100): ");
            string input = Console.ReadLine() ?? "";

            var result = from p in _passengers
                         where p.LuggageWeight < 20 && p.FlightNumber == input
                         select p;

            if (!result.Any()) Console.WriteLine("No passengers found.");
            foreach (var item in result) Console.WriteLine(item);
            Console.WriteLine();
        }

        private static void Task3_Sort()
        {
            Console.WriteLine("3. Sort: Passengers by Luggage Weight Descending (Method Syntax)");
            var result = _passengers.OrderByDescending(p => p.LuggageWeight);
            foreach (var item in result) Console.WriteLine(item);
            Console.WriteLine();
        }

        private static void Task4_Count()
        {
            Console.WriteLine("4. Count: Number of passengers with luggage > 10kg (Method Syntax)");
            int count = _passengers.Count(p => p.LuggageWeight > 10);
            Console.WriteLine($"Count: {count}");
            Console.WriteLine();
        }

        private static void Task5_Aggregates()
        {
            Console.WriteLine("5. Aggregates: Max, Avg, Sum of Luggage Weight (Method Syntax)");
            double max = _passengers.Max(p => p.LuggageWeight);
            double avg = _passengers.Average(p => p.LuggageWeight);
            double sum = _passengers.Sum(p => p.LuggageWeight);

            Console.WriteLine($"Max: {max:F2} kg");
            Console.WriteLine($"Average: {avg:F2} kg");
            Console.WriteLine($"Sum: {sum:F2} kg");
            Console.WriteLine();
        }

        private static void Task6_LetOperator()
        {
            Console.WriteLine("6. Let: Calculate Overweight Cost (Standard 20kg, $10 per extra kg) (Query Syntax)");
            var result = from p in _passengers
                         let overweight = p.LuggageWeight - 20
                         where overweight > 0
                         let cost = overweight * 10
                         select new { p.Name, Overweight = overweight, Cost = cost };

            foreach (var item in result) Console.WriteLine($"Passenger: {item.Name}, Overweight: {item.Overweight:F1}kg, Fee: ${item.Cost:F2}");
            Console.WriteLine();
        }

        private static void Task7_Grouping()
        {
            Console.WriteLine("7. Grouping: Passengers by Flight Number (Query Syntax)");
            var groups = from p in _passengers
                         group p by p.FlightNumber;

            foreach (var g in groups)
            {
                Console.WriteLine($"Flight: {g.Key}");
                foreach (var p in g)
                {
                    Console.WriteLine($"  - {p.Name} ({p.LuggageWeight}kg)");
                }
            }
            Console.WriteLine();
        }

        private static void Task8_Join()
        {
            Console.WriteLine("8. Join: Passengers info with Flight Destination (Query Syntax)");
            var result = from p in _passengers
                         join f in _flights on p.FlightNumber equals f.FlightNumber
                         select new { p.Name, f.Destination, f.Airline };

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name} is flying to {item.Destination} by {item.Airline}");
            }
            Console.WriteLine();
        }

        private static void Task9_GroupJoin()
        {
            Console.WriteLine("9. GroupJoin: Flights and their passenger lists (Method Syntax)");
            var result = _flights.GroupJoin(
                _passengers,
                f => f.FlightNumber,
                p => p.FlightNumber,
                (flight, passengers) => new { Flight = flight, Passengers = passengers }
            );

            foreach (var item in result)
            {
                Console.WriteLine($"Flight {item.Flight.FlightNumber} ({item.Flight.Destination}): {item.Passengers.Count()} passengers");
                foreach (var p in item.Passengers)
                {
                    Console.WriteLine($"  * {p.Name}");
                }
            }
            Console.WriteLine();
        }

        private static void Task10_All()
        {
            Console.WriteLine("10. All: Do all passengers have luggage weight > 0? (Method Syntax)");
            bool allHaveLuggage = _passengers.All(p => p.LuggageWeight > 0);
            Console.WriteLine($"Result: {allHaveLuggage}");
            Console.WriteLine();
        }

        private static void Task11_Any()
        {
            Console.WriteLine("11. Any: Is there any passenger on a specific flight? (Method Syntax)");
            Console.Write("Enter Flight Number to check: ");
            string input = Console.ReadLine() ?? "";

            bool exists = _passengers.Any(p => p.FlightNumber == input);
            Console.WriteLine($"Exists: {exists}");
            Console.WriteLine();
        }
    }
}