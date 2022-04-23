using System.Collections.Generic;
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
        [SerializeField]
        private BossPhaseSwitching _phaseSwitching;

        [Header("Attack")]
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        private Transform _armShootPoint;
        [SerializeField]
        private BossAttack _bossAttack;
        [SerializeField]
        private BossShooter _bossShooter;

        [SerializeField]
        private BossAnimator _bossAnimator;

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

            _phaseSwitching.Init(_boss, _phases);
            
            _bossAttack.Init(_phaseSwitching, _bossAnimator, player.transform, _bossShooter);

            _boss.Init(_bossAnimator, new Health(_healthValue), _bossAttack);
        }
    }
}