using System;

namespace Lab2.Classes
{
    // 1. Использование собственных классов
    public class Ticket
    {
        public decimal Price { get; set; }
        public string Destination { get; set; }

        public Ticket(string destination, decimal price)
        {
            Destination = destination;
            Price = price;
        }

        // 11. Использование перегруженных операторов
        public static Ticket operator +(Ticket t, decimal fee)
        {
            return new Ticket(t.Destination, t.Price + fee);
        }

        public override string ToString()
        {
            return $"Ticket to {Destination}: ${Price}";
        }
    }
}