using System;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    [Serializable]
    public class Health : IHealth
    {
        public event Action Died;
        public event Action<int> HealthChanged;

        [SerializeField]
        private int _value;

        public Health(int value) => _value = value;

        public int Value
        {
            get => _value;
            private set
            {
                _value = value;
                HealthChanged?.Invoke(_value);
            }
        }

        public void TakeDamage(int damage)
        {
            if (_value <= 0)
                return;
            
            _value -= damage;
            Value = Mathf.Clamp(_value, 0, 100000);
            if (_value <= 0)
                Died?.Invoke();
        }

        public void IncreaseHealthValue(int value) => Value += value;
    }

    public interface IHealth
    {
        public event Action<int> HealthChanged; 
        public int Value { get; }
    }
}