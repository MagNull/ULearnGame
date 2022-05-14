using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public abstract class BossPhaseSwitching : SerializedMonoBehaviour
    {
        public event Action PhaseSwitched;
        
        [SerializeField]
        private Dictionary<int, string[]> _phases = new();
        
        [SerializeField]
        private float _phaseSwitchingDuration;
        [SerializeField]
        private BossPhase _currentPhase;
        private Boss _boss;
        private BossPhase _switchingPhase;
        private float _healthPercent;

        public BossPhase CurrentPhase => _currentPhase;

        [Inject]
        public void Init(Boss boss)
        {
            _boss = boss;
            _currentPhase = GetFirstPhase(_phases);
            _healthPercent = (float)_boss.GetHealthValue() / 100;
        }

        public void CheckPhase()
        {
            var healthPercentage = _boss.GetHealthValue() / _healthPercent;
            if (CurrentPhase.NextPhase != null && healthPercentage < CurrentPhase.NextPhase.HealthThreshold)
                StartCoroutine(PhaseSwitching());
        }

        private BossPhase GetFirstPhase(Dictionary<int, string[]> phases)
        {
            _switchingPhase = new BossPhase(phases[-1], -1, null);
            phases.Remove(-1);
            phases = phases
                .OrderBy(phase => phase.Key)
                .ToDictionary(phase => phase.Key, phase => phase.Value);
            var lastPhase = new BossPhase(phases.First().Value, phases.First().Key, null);
            foreach (var phase in phases.Skip(1).SkipLast(1))
            {
                lastPhase = new BossPhase(phase.Value, phase.Key, lastPhase);
            }

            return new BossPhase(phases.Last().Value, phases.Last().Key, lastPhase);
        }

        private IEnumerator PhaseSwitching()
        {
            var prevPhase = _currentPhase;
            _currentPhase = _switchingPhase;
            OnPhaseSwitchingStarted();
            PhaseSwitched?.Invoke();

            yield return new WaitForSeconds(_phaseSwitchingDuration);

            OnPhaseSwitchingEnded();
            _currentPhase = prevPhase.NextPhase;
        }

        protected abstract void OnPhaseSwitchingStarted();
        protected abstract void OnPhaseSwitchingEnded();
    }
}