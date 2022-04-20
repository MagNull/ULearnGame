using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    public class BossAnimator
    {
        private readonly Animator _animator;
        private readonly int _dieHash = Animator.StringToHash("Die");

        public BossAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void StartAttack(string attackName)
        {
            _animator.SetTrigger(attackName);
        }

        public void OnDied()
        {
            _animator.SetTrigger(_dieHash);
        }
    }
}