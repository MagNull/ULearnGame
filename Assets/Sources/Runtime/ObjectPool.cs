using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class ObjectPool<T> where T : IPoolObject
    {
        private readonly Stack<IPoolObject> _pool;
        private readonly IFactory<T> _factory;

        public ObjectPool(int capacity, IFactory<T> factory)
        {
            _pool = new Stack<IPoolObject>();
            _factory = factory;
            FillPool(capacity);
        }

        public T Get()
        {
            T result;
            if (_pool.Any())
            {
                result = (T)_pool.Pop();
            }
            else
            {
                result = _factory.Create();
                _pool.Push(result);
            }
            
            result.Enable();
            return result;
        }

        private void FillPool(int capacity)
        {
            for (var i = 0; i < capacity; i++)
            {
                var obj = _factory.Create();
                obj.Disable();
                _pool.Push(obj);
                obj.BecameUnused += OnObjectBecameUnused;
            }
        }

        private void OnObjectBecameUnused(IPoolObject poolObject)
        {
            _pool.Push(poolObject);
            poolObject.Disable();
        }
    }
}