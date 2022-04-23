using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    public class Player : MonoBehaviour, IDamageable
    {
        public event Action<int> Damaged;
        [SerializeReference]
        private IMovement _movement;
        [SerializeReference]
        private IShooter _shooter;
        [SerializeField]
        private Health _health;
        private PlayerAnimator _animator;

        public void Init(IMovement movement, IShooter shooter, PlayerAnimator animator, Health health)
        {
            _movement = movement;
            _shooter = shooter;
            _health = health;
            _animator = animator;
            enabled = true;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            Damaged?.Invoke(_health.Value);
        }

        private void OnDied()
        {
            Debug.Log(gameObject.name + " died.");
        }

        private void OnEnable()
        {
            _movement.Moved += _animator.OnMoved;
            _health.Died += OnDied;
        }

        private void OnDisable()
        {
            _movement.Moved -= _animator.OnMoved;
            _health.Died -= OnDied;
        }
    }
}