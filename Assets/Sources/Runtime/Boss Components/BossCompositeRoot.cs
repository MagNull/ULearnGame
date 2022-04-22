using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Runtime.Player_Components;
using Sources.Runtime.Utils;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{

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
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        private Transform _armShootPoint;
        [SerializeField] private BossAttack _bossAttack;
        [SerializeField] private BossShooter _bossShooter;
        
        [SerializeField] private BossAnimator _bossAnimator;

        private void Awake()
        {
            Compose();
        }

        private void Compose()
        {
            var player = FindObjectOfType<Player>();
            
            _bossShooter.Init(player.transform, 
                new ObjectPool<GolemArm>(3, _projectileFactory.Create<GolemArm, Boss>),
                new ObjectPool<Projectile>(100, _projectileFactory.Create<Projectile, Boss>));
            
            _bossAttack.Init(GetBossPhases(), _bossAnimator, _boss, player.transform,
                _bossShooter);
            
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