using Sources.Runtime.Interfaces;
using Sources.Runtime.Utils;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    public class BossShooter : MonoBehaviour, IShooter
    {
        [SerializeField]
        private Transform _armShootPoint;
        [SerializeField]
        private float _projectileSpeed = 5;

        [SerializeField]
        private ProjectileFactory _playerProjectileAbstractFactory;

        private ObjectPool<GolemsArm> _armPool;
        private ObjectPool<SimpleProjectile> _prjectilePool;

        private void Awake()
        {
            _armPool = new ObjectPool<GolemsArm>(2, _playerProjectileAbstractFactory.Create<GolemsArm, Boss>);
            _prjectilePool =
                new ObjectPool<SimpleProjectile>(100, _playerProjectileAbstractFactory.Create<SimpleProjectile, Boss>);
        }

        public void Shoot()
        {
            var projectile = _armPool.Get();
            projectile.transform.position = _armShootPoint.position;
            Vector2 shootDirection = Vector2.right;
            projectile.SetVelocity(shootDirection.normalized * _projectileSpeed);
            projectile.Pool = _prjectilePool;
        }
    }
}