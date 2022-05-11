using System;
using Sources.Runtime.Interfaces;
using Sources.Runtime.UI___HUD;
using UnityEngine;
using Zenject;

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
        private StopScreen _stopScreen;

        [Inject]
        private void Init(IMovement movement, IShooter shooter, PlayerAnimator animator,
            [Inject(Id = "Player")] Health health,
            [Inject(Id = "Die Screen")] StopScreen dieScreen)
        {
            _movement = movement;
            _shooter = shooter;
            _health = health;
            _animator = animator;
            _stopScreen = dieScreen;
            _stopScreen.gameObject.SetActive(false);
            enabled = true;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            Damaged?.Invoke(_health.Value);
        }

        private void OnDied()
        {
            Time.timeScale = 0;
            _stopScreen.gameObject.SetActive(true);
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