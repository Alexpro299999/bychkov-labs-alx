using System;

namespace Lab2.Classes
{
    // 1. Использование собственных классов
    // 9. Использование наследования
    public class Train : Transport
    {
        public int CarriageCount { get; set; }

        public Train(string id, double speed, int carriages) : base(id, speed)
        {
            CarriageCount = carriages;
        }

        // 10. Использование переопределений методов
        public override void Move()
        {
            Console.WriteLine($"Train {_id} is moving on rails.");
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Train ID: {Id}, Speed: {Speed}, Carriages: {CarriageCount}");
        }
    }
}