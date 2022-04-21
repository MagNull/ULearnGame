using System;
using Sources.Runtime.Boss_Components;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using UnityEngine;

namespace Sources.Runtime
{
    public class ProjectileFactory : MonoBehaviour, IFactory<Projectile>
    {
        [SerializeField]
        private SimpleProjectile _playerSimpleProjectile;
        [SerializeField]
        private SimpleProjectile _enemySimpleProjectile;
        [SerializeField]
        private GolemsArm _golemsArmPrefab;

        private readonly Type _bossType = typeof(Boss);
        private readonly Type _playerType = typeof(Player);
        private readonly Type _simpleProjectileType = typeof(SimpleProjectile);
        private readonly Type _golemsArmType = typeof(GolemsArm);

        public TProjectile Create<TProjectile, TProductOwner>() where TProjectile : Projectile
        {
            var projectileT = typeof(TProjectile);
            var ownerT = typeof(TProductOwner);

            if (projectileT == _simpleProjectileType)
            {
                if (ownerT == _bossType)
                    return Instantiate(_enemySimpleProjectile, transform) as TProjectile;
                if (ownerT == _playerType)
                    return Instantiate(_playerSimpleProjectile, transform) as TProjectile;
            }
            if (projectileT == _golemsArmType)
                return Instantiate(_golemsArmPrefab, transform) as TProjectile;
            
            throw new Exception("Projectile or Owner type is unknown");
        }
    }
}