using System;
using System.Collections.Generic;

namespace Scripts
{
    public class ObjectPool<T> where T : new()
    {
        private Queue<T> _objects = null;

        public ObjectPool(int capacity)
        {
            _objects = new Queue<T>(capacity);

            for (int i = 0; i < capacity; ++i)
            {
                _objects.Enqueue(new T());
            }
        }

        public T Get()
        {
            if (_objects.Count > 0)
            {
                return _objects.Dequeue();
            }

            return new T();
        }

        public void Return(T obj)
        {
            _objects.Enqueue(obj);
        }
    }
}