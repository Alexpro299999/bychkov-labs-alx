using System;

namespace Lab2.Generics
{
    // 14. Использование наследования обобщений
    public class LuggageStorage<T> : Storage<T>
    {
        public void DisplayAll()
        {
            Console.WriteLine("Luggage Storage Contents:");
            foreach (var item in _items)
            {
                Console.WriteLine($" - {item}");
            }
        }
    }
}