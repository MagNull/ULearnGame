using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    public class BossAttack
    {
        private BossPhase _currentPhase;
        private readonly BossAnimator _bossAnimator;

        public BossAttack(BossPhase currentPhase, BossAnimator bossAnimator, Boss boss)
        {
            boss.Damaged += OnDamaged;
            _currentPhase = currentPhase;
            _bossAnimator = bossAnimator;
        }

        private void OnDamaged(int health)
        {
            if (health <= _currentPhase.NextPhase.HealthThreshold)
                _currentPhase = _currentPhase.NextPhase;
        }

        public void StartAttack()
        {
            _bossAnimator.StartAttack(_currentPhase.AttacksPool[Random.Range(0, _currentPhase.AttacksPool.Length)]);
        }
    }
}