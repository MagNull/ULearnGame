using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Runtime.Player_Components;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    [RequireComponent(typeof(Animator))]
    public class BossCompositeRoot : SerializedMonoBehaviour
    {
        [SerializeField]
        private Boss _boss;
        [SerializeField]
        private int _healthValue = 2;
        [Header("Phases")]
        [SerializeField]
        private Dictionary<int, string[]> _phases = new();

        [Header("Attack")]
        private Transform _armShootPoint;
        private BossAttack _bossAttack;

        private BossAnimator _bossAnimator;

        private void Awake()
        {
            Compose();
        }

        private void Compose()
        {
            _bossAnimator = new BossAnimator(GetComponent<Animator>());
            _bossAttack = new BossAttack(GetBossPhases(), _bossAnimator, _boss);
            _boss.Init(_bossAnimator, new Health(_healthValue), _bossAttack);
        }

        private BossPhase GetBossPhases()
        {
            _phases = _phases
                .OrderBy(phase => phase.Key)
                .ToDictionary(phase => phase.Key, phase => phase.Value);
            var lastPhase = new BossPhase(_phases.First().Value, _phases.First().Key, null);
            foreach (var phase in _phases.SkipLast(1))
            {
                lastPhase = new BossPhase(phase.Value, phase.Key, lastPhase);
            }

            return new BossPhase(_phases.Last().Value, _phases.Last().Key, lastPhase);
        }
    }
}