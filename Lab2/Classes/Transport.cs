using System;
using Lab2.Interfaces;

namespace Lab2.Classes
{
    // 6. Использование абстрактных классов
    public abstract class Transport : IIdentifiable
    {
        // 7. Использование принципов инкапсуляции (protected)
        protected string _id;
        protected double _speed;

        // 2. Использование конструкторов классов с параметрами
        public Transport(string id, double speed)
        {
            _id = id;
            Speed = speed;
        }

        // 3. Использование свойств
        public string Id => _id;

        // 4. Использование свойств с логикой в set блоке
        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value < 0)
                    _speed = 0;
                else
                    _speed = value;
            }
        }

        // 6. Абстрактный метод
        public abstract void Move();

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Transport ID: {_id}, Speed: {_speed}");
        }
    }
}