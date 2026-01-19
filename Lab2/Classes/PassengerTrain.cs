using System;

namespace Lab2.Classes
{
    // 9. Использование наследования
    public class PassengerTrain : Train
    {
        public bool HasWifi { get; set; }

        public PassengerTrain(string id, double speed, int carriages, bool hasWifi)
            : base(id, speed, carriages)
        {
            HasWifi = hasWifi;
        }

        // 10. Использование переопределений методов
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Wi-Fi Available: {HasWifi}");
        }
    }
}