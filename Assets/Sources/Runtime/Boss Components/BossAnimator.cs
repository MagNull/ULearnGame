using System;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    [RequireComponent(typeof(Animator))]
    public class BossAnimator : MonoBehaviour
    {
        private Animator _animator;
        private readonly int _dieHash = Animator.StringToHash("Die");
        private readonly int _idleHash = Animator.StringToHash("Idle");

        public void IncreaseAttackSpeedMulti(float multiplier)
        {
            _animator.speed *= multiplier;
        }

        public void TriggerAttack(string attackName)
        {
            _animator.SetTrigger(attackName);
        }

        public void OnDied()
        {
            _animator.SetTrigger(_dieHash);
        }

        public void OnAttackEnded()
        {
            _animator.SetTrigger(_idleHash);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}