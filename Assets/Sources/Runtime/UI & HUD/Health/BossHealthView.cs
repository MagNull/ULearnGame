using Sources.Runtime.Boss_Components;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Runtime.Player_Components
{
    [RequireComponent(typeof(Slider))]
    public class BossHealthView : MonoBehaviour
    {
        private Slider _healthSlider;

        [Inject]
        private void Init(Boss boss, [Inject(Id = "Boss")]Health health)
        {
            _healthSlider = GetComponent<Slider>();
            _healthSlider.maxValue = health.Value;
            _healthSlider.value = health.Value;
            boss.Damaged += OnDamaged;
        }

        private void OnDamaged(int health) => _healthSlider.value = health;
    }
    
}