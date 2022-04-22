using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Runtime.Boss_Components
{
    public class BossAttack : MonoBehaviour
    {
        [Header("Jump Attack")]
        [SerializeField]
        private float _jumPower = 10;
        [SerializeField]
        private float _jumpDuration = 2;

        private BossPhase _currentPhase;
        private BossAnimator _bossAnimator;
        private BossShooter _shooter;
        private Collider2D _collider2D;
        private Transform _player;

        public void Init(BossPhase currentPhase, BossAnimator animator, Boss boss, Transform player,
            BossShooter shooter)
        {
            boss.Damaged += OnDamaged;
            _currentPhase = currentPhase;
            _bossAnimator = animator;
            _player = player;
            _shooter = shooter;
            _collider2D = GetComponent<Collider2D>();
            enabled = true;
        }

        public void StartAttack() =>
            _bossAnimator.TriggerAttack(_currentPhase.AttacksPool[Random.Range(0, _currentPhase.AttacksPool.Length)]);

        public void JumpAttack()
        {
            _collider2D.enabled = false;
            var jump = transform.DOJump(_player.position, _jumPower, 1, _jumpDuration);
            jump.onComplete += () =>
            {
                _bossAnimator.OnAttackEnded();
                _shooter.RingShoot();
                _collider2D.enabled = true;
            };
        }

        private void OnDamaged(int health)
        {
            if (health <= _currentPhase.NextPhase.HealthThreshold)
                _currentPhase = _currentPhase.NextPhase;
        }
    }
}