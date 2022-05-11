using System.Collections.Generic;
using Sources.Runtime.Player_Components;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public abstract class BossCompositionRoot<TShooter, TAttack> : MonoInstaller 
        where TShooter : BossShooter
        where TAttack : BossAttack<TShooter>
    {
        private Boss _boss;
        [Header("Health")]
        [SerializeField]
        private int _healthValue = 2;
        [SerializeField]
        private SliderHealthView _healthView;
        [Header("Phases")]
        [SerializeField]
        private Dictionary<int, string[]> _phases = new();
        private BossPhaseSwitching _phaseSwitching;

        [Header("Attack")]
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        private Transform _armShootPoint;
        private TShooter _bossShooter;

        [SerializeField]
        private BossAnimator _bossAnimator;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _boss = GetComponent<Boss>();
            _phaseSwitching = GetComponent<BossPhaseSwitching>();
            _bossShooter = GetComponent<TShooter>();
        }

        public override void InstallBindings()
        {
            Init();
            Container.Bind<Boss>().FromInstance(_boss).AsSingle();
            Container.Bind<TShooter>().FromInstance(_bossShooter).AsSingle();
            Container.Bind<BossPhaseSwitching>().FromInstance(_phaseSwitching).AsSingle();
            Container.Bind<BossAnimator>().FromInstance(_bossAnimator).AsSingle();
            Container.Bind<Health>().WithId("Boss").FromInstance(new Health(_healthValue));
            Container.Bind<ProjectileFactory>().FromInstance(_projectileFactory);
        }
    }
}