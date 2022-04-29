using Sources.Runtime.Boss_Components;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Player_Components
{
    [RequireComponent(typeof(Slider))]
    public class SliderHealthView : MonoBehaviour
    {
        private Slider _healthSlider;

        public void Init(Boss boss, int healthValue)
        {
            _healthSlider = GetComponent<Slider>();
            _healthSlider.maxValue = healthValue;
            _healthSlider.value = healthValue;
            boss.Damaged += OnDamaged;
        }

        private void OnDamaged(int health) => _healthSlider.value = health;
    }
    
}