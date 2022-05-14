using Sources.Runtime.Player_Components;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    [RequireComponent(
        typeof(BossPhaseSwitching), 
        typeof(Boss), 
        typeof(BossAnimator))]
    public abstract class BossCompositionRoot<TShooter> : MonoInstaller
        where TShooter : BossShooter
    {
        private Boss _boss;
        [Header("Health")]
        [SerializeField]
        private int _healthValue = 2;

        private BossPhaseSwitching _phaseSwitching;

        [Header("Attack")]
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        private Transform _armShootPoint;
        private TShooter _bossShooter;
        
        private BossAnimator _bossAnimator;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _boss = GetComponent<Boss>();
            _bossAnimator = GetComponent<BossAnimator>();
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
            Container.Bind<Transform>().WithId("Player").FromInstance(FindObjectOfType<Player>().transform);
        }
    }
}