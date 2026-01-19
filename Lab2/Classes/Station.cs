using System;
using System.Collections.Generic;

namespace Lab2.Classes
{
    public class Station
    {
        // 8. Использование статических элементов
        public static string StationName = "Central Station";

        // 17. Использование композиции классов (платформы часть станции)
        private List<Platform> _platforms = new List<Platform>();

        // 16. Использование агрегации классов (поезда приходят и уходят, существуют отдельно)
        private List<Train> _trains = new List<Train>();

        public Station()
        {
            // Composition: Station creates its platforms
            _platforms.Add(new Platform(1, true));
            _platforms.Add(new Platform(2, false));
            _platforms.Add(new Platform(3, true));
        }

        public void Arrive(Train train)
        {
            _trains.Add(train);
        }

        // 5. Использование индексаторов
        public Platform this[int index]
        {
            get
            {
                if (index >= 0 && index < _platforms.Count)
                    return _platforms[index];
                return null;
            }
        }

        // 13. Использование обобщенных методов
        public void Announce<T>(T message)
        {
            Console.WriteLine($"[ANNOUNCEMENT at {StationName}]: {message}");
        }

        public void ShowTrains()
        {
            Console.WriteLine("Trains at station:");
            foreach (var t in _trains)
            {
                t.DisplayInfo();
            }
        }
    }
}