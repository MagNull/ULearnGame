using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public abstract class BossAttack<TShooter> : MonoBehaviour where TShooter : BossShooter
    {
        [SerializeField]
        private float _phaseSwitchAttackSpeedMulti;
        protected BossAnimator _bossAnimator;
        protected TShooter _shooter;
        private BossPhaseSwitching _phaseSwitching;

        protected float PhaseSwitchAttackSpeedMulti => _phaseSwitchAttackSpeedMulti;
        
        [Inject]
        public void Init(BossPhaseSwitching phaseSwitching, BossAnimator animator, TShooter shooter)
        {
            _phaseSwitching = phaseSwitching;
            _bossAnimator = animator;
            _shooter = shooter;
            _phaseSwitching.PhaseSwitched += IncreaseAttackSpeed;
            enabled = true;
        }

        public void StartAttack()
        {
            if (_phaseSwitching.CurrentPhase.AttacksPool.Length > 0)
                _bossAnimator.TriggerAttack(
                    _phaseSwitching.CurrentPhase.AttacksPool[
                        Random.Range(0, _phaseSwitching.CurrentPhase.AttacksPool.Length)]);
        }

        protected abstract void IncreaseAttackSpeed();
    }
}