namespace Lab5
{
    public class Passenger
    {
        public string Name { get; set; } = string.Empty;
        public double LuggageWeight { get; set; }
        public string FlightNumber { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name} (Luggage: {LuggageWeight}kg, Flight: {FlightNumber})";
        }
    }

    public class Flight
    {
        public string FlightNumber { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string Airline { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Flight {FlightNumber} to {Destination} ({Airline})";
        }
    }
}