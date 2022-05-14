using System;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public class Boss: MonoBehaviour, IDamageable
    {
        public event Action<int> Damaged;

        [SerializeField]
        private Health _health;
        private BossAnimator _animator;

        [Inject]
        public void Init(BossAnimator animator, [Inject(Id = "Boss")]Health health)
        {
            _health = health;
            _animator = animator;
            enabled = true;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            Damaged?.Invoke(_health.Value);
        }

        public int GetHealthValue() => _health.Value;

        private void OnEnable()
        {
            _health.Died += _animator.OnDied;
        }

        private void OnDisable()
        {
            _health.Died -= _animator.OnDied;
        }

        private void OnDied()
        {
            Debug.Log(gameObject.name + " died.");
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}