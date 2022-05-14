using Sources.Runtime.Player_Components;
using Sources.Runtime.Utils;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public class GolemShooter : BossShooter
    {
        [Header("Arm Shoot")]
        [SerializeField]
        private Transform _armShootPoint;

        [Header("Ring shoot")]
        [SerializeField]
        private int _ringProjectilesCount;
        private float _projectilesAngle;

        private ObjectPool<GolemArm> _armPool;
        private Transform _playerTransform;

        [Inject]
        public void Init([Inject(Id = "Player")]Transform playerTransform, ProjectileFactory factory)
        {
            base.Init(factory);
            _playerTransform = playerTransform;
            _armPool = new ObjectPool<GolemArm>(2, factory.Create<GolemArm, Boss>);
            _projectilesAngle = 2 * Mathf.PI / _ringProjectilesCount * Mathf.Rad2Deg;
            enabled = true;
        }

        public void ShootArm()
        {
            var arm = _armPool.Get();
            Vector2 shootDirection = _playerTransform.transform.position - _armShootPoint.position;
            Shoot(_armShootPoint.position, shootDirection.normalized, arm);
            arm.Init(this);
        }

        public void RingShoot()
        {
            RingShoot(transform.position);
        }

        public void RingShoot(Vector3 origin)
        {
            var additionalAngle = Random.Range(-Mathf.PI, Mathf.PI) * Mathf.Rad2Deg;
            for (var i = 0; i < _ringProjectilesCount; i++)
            {
                Vector2 direction =
                    Quaternion.Euler(0, 0,_projectilesAngle * i + additionalAngle) 
                    * Vector2.right;
                Shoot(origin, direction.normalized);
            }
        }
    }
}