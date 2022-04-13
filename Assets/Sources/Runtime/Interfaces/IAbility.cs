using System;

namespace Sources.Runtime.Interfaces
{
    public interface IAbility
    {
        public float CoolDown { get; }
        
        public event Action Casted;
        void Cast();
    }
}