using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Runtime.Player_Components;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public abstract class BossCompositionRoot<TShooter> : MonoInstaller where TShooter : BossShooter
    {
        [Header("Bosses Spawn Configs")]
        [SerializeField]
        private List<Boss> _bossesByLevel = new();
        [SerializeField]
        private Transform _bossSpawnPoint;
        
        private Boss _boss;
        private BossPhaseSwitching _phaseSwitching;

        [Header("Attack")]
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        private Transform _armShootPoint;
        private TShooter _bossShooter;
        
        private BossAnimator _bossAnimator;
        
        public void Init()
        {
            var level = PlayerPrefs.GetInt("Boss Level");
            _boss = Instantiate(_bossesByLevel[level - 1], _bossSpawnPoint.position, Quaternion.identity);
            _bossAnimator = _boss.GetComponent<BossAnimator>();
            _phaseSwitching = _boss.GetComponent<BossPhaseSwitching>();
            _bossShooter = _boss.GetComponent<TShooter>();
        }

        public override void InstallBindings()
        {
            Container.Bind<Boss>().FromInstance(_boss).AsSingle();
            Container.Bind<TShooter>().FromInstance(_bossShooter).AsSingle();
            Container.Bind<BossPhaseSwitching>().FromInstance(_phaseSwitching).AsSingle();
            Container.Bind<BossAnimator>().FromInstance(_bossAnimator).AsSingle();
            Container.Bind<Health>().WithId("Boss").FromInstance(_boss.Health);
            Container.Bind<ProjectileFactory>().FromInstance(_projectileFactory);
            Container.Bind<Transform>().WithId("Player").FromInstance(FindObjectOfType<Player>().transform);
        }
    }
}