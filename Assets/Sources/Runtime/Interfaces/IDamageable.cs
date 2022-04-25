using System;

namespace Sources.Runtime.Interfaces
{
    public interface IDamageable
    {
        public event Action<int> Damaged;
        
        void TakeDamage(int damage);
    }
}