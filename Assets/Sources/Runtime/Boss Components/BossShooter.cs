using Sources.Runtime.Player_Components;
using Sources.Runtime.Utils;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    public class BossShooter : MonoBehaviour
    {
        [SerializeField]
        private float _projectileSpeed = 5;
        [SerializeField]
        private Transform _rotationTarget;

        [Header("Arm Shoot")]
        [SerializeField]
        private Transform _armShootPoint;

        [Header("Ring shoot")]
        [SerializeField]
        private int _ringProjectilesCount;
        private float _projectilesAngle;

        private ObjectPool<GolemArm> _armPool;
        private ObjectPool<Projectile> _projectilePool;
        private Transform _player;

        public void Init(Transform player, ObjectPool<GolemArm> armPool, ObjectPool<Projectile> projectilePool)
        {
            _player = player;
            _armPool = armPool;
            _projectilePool = projectilePool;
            _projectilesAngle = 2 * Mathf.PI / _ringProjectilesCount * Mathf.Rad2Deg;
            enabled = true;
        }

        public void ShootArm()
        {
            var arm = _armPool.Get();
            arm.transform.position = _armShootPoint.position;
            Vector2 shootDirection = _player.transform.position - arm.transform.position;
            arm.SetVelocity(shootDirection.normalized * _projectileSpeed);
            arm.Init(_projectilePool);
        }

        public void RingShoot()
        {
            for (var i = 0; i < _ringProjectilesCount; i++)
            {
                var projectile = _projectilePool.Get();
                Vector2 direction =
                    Quaternion.Euler(0, 0,_projectilesAngle * i) 
                    * Vector2.right;
                projectile.transform.position = transform.position;
                projectile.SetVelocity(_projectileSpeed * direction);
            }
        }
    }
}