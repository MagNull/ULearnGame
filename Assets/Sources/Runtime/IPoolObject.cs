using System;

namespace DefaultNamespace
{
    public interface IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        
        void Enable();
        void Disable();
    }
}