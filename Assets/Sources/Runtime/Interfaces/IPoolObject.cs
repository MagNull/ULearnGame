using System;

namespace Sources.Runtime.Interfaces
{
    public interface IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        
        void Enable();
        void Disable();
    }
}