using System.Collections.Generic;

namespace Lab5
{
    public static class DataSeeder
    {
        public static List<Flight> GetFlights()
        {
            return new List<Flight>
            {
                new Flight { FlightNumber = "SU-100", Destination = "Moscow", Airline = "Aeroflot" },
                new Flight { FlightNumber = "DL-202", Destination = "New York", Airline = "Delta" },
                new Flight { FlightNumber = "AF-303", Destination = "Paris", Airline = "Air France" },
                new Flight { FlightNumber = "LH-404", Destination = "Berlin", Airline = "Lufthansa" },
                new Flight { FlightNumber = "BA-505", Destination = "London", Airline = "British Airways" }
            };
        }

        public static List<Passenger> GetPassengers()
        {
            return new List<Passenger>
            {
                new Passenger { Name = "Ivanov I.", LuggageWeight = 15.5, FlightNumber = "SU-100" },
                new Passenger { Name = "Petrov P.", LuggageWeight = 23.0, FlightNumber = "SU-100" },
                new Passenger { Name = "Smith J.", LuggageWeight = 8.0, FlightNumber = "DL-202" },
                new Passenger { Name = "Doe J.", LuggageWeight = 30.5, FlightNumber = "DL-202" },
                new Passenger { Name = "Dupont M.", LuggageWeight = 12.0, FlightNumber = "AF-303" },
                new Passenger { Name = "Muller H.", LuggageWeight = 19.5, FlightNumber = "LH-404" },
                new Passenger { Name = "Bond J.", LuggageWeight = 5.0, FlightNumber = "BA-505" },
                new Passenger { Name = "Watson J.", LuggageWeight = 25.0, FlightNumber = "BA-505" },
                new Passenger { Name = "Sidorov S.", LuggageWeight = 10.0, FlightNumber = "SU-100" },
                new Passenger { Name = "Johnson A.", LuggageWeight = 35.0, FlightNumber = "DL-202" }
            };
        }
    }
}