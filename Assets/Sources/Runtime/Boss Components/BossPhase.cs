using System;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    [Serializable]
    public class BossPhase
    {
        [SerializeField]
        private string[] _attacksPool;
        [SerializeField]
        private int _healthThreshold;
        private BossPhase _nextPhase;

        public BossPhase NextPhase => _nextPhase;

        public int HealthThreshold => _healthThreshold;

        public string[] AttacksPool => _attacksPool;

        public BossPhase(string[] attackPool, int healthThreshold, BossPhase nextPhase)
        {
            _attacksPool = attackPool;
            _healthThreshold = healthThreshold;
            _nextPhase = nextPhase;
        }
    }
}