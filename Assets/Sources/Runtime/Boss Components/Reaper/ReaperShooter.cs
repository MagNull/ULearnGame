using System.Collections.Generic;
using Sources.Runtime.Utils;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components.Reaper
{
    public class ReaperShooter : BossShooter
    {
        [SerializeField]
        private ReaperSummon _reaperSummon;
        private Transform _playerTransform;
        private ObjectPool<ReaperSummon> _summonPool;

        [Inject]
        public void Init([Inject(Id = "Player")]Transform playerTransform, ProjectileFactory factory)
        {
            base.Init(factory);
            _playerTransform = playerTransform;
            _summonPool = new ObjectPool<ReaperSummon>(0, factory.Create<ReaperSummon, Boss>);
        }
        
        public void ShootSummon()
        {
            var direction = Quaternion.AngleAxis(Random.Range(0, 360), transform.forward) * transform.up;
            Shoot(transform.position, direction, _summonPool.Get());
        }
        
        public void ShootChargedProjectiles(List<(Projectile, Vector3)> projectiles, float delay)
        {
            for (var i = 0; i < projectiles.Count; i++)
            {
                Shoot(projectiles[i].Item1.transform.position, projectiles[i].Item2,
                    projectiles[i].Item1, delay * i);
            }
        }
    }
}