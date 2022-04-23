using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    public class BossPhaseSwitching : MonoBehaviour
    {
        [SerializeField]
        private GameObject _spikes;
        [SerializeField]
        private float _phaseSwitchingDuration;
        [SerializeField]
        private BossPhase _currentPhase;
        private Boss _boss;
        private BossPhase _switchingPhase;

        public BossPhase CurrentPhase => _currentPhase;

        public void Init(Boss boss, Dictionary<int, string[]> phases)
        {
            _boss = boss;
            _currentPhase = GetBossPhases(phases);
        }

        public void CheckPhase()
        {
            var health = _boss.GetHealthValue();
            if (CurrentPhase.NextPhase != null && health < CurrentPhase.NextPhase.HealthThreshold)
                StartCoroutine(PhaseSwitching());
        }

        private BossPhase GetBossPhases(Dictionary<int, string[]> phases)
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
            var prevPhase = CurrentPhase;
            _currentPhase = _switchingPhase;
            _spikes.SetActive(true);

            yield return new WaitForSeconds(_phaseSwitchingDuration);

            _currentPhase = prevPhase.NextPhase;
            _spikes.SetActive(false);
        }
    }
}