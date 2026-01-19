using System;
using System.Collections.Generic;

namespace Lab2.Generics
{
    // 12. Использование обобщений
    public class Storage<T>
    {
        protected List<T> _items = new List<T>();

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T Get(int index)
        {
            if (index >= 0 && index < _items.Count)
                return _items[index];
            return default(T);
        }
    }
}