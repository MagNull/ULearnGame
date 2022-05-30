using System;
using Sources.Runtime.Boss_Components;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    public class GolemArm : Projectile
    {
        private BossShooter _bossShooter;
        [Header("Additional projectile")]
        [SerializeField]
        private float _offset = 1;
        [SerializeField]
        private int _count = 1;

        private float _projectilesAngle;

        public void Init(BossShooter bossShooter)
        {
            _bossShooter = bossShooter;
        }

        private void Start()
        {
            _projectilesAngle = Mathf.PI / _count * Mathf.Rad2Deg;
        }

        private void OnEnable()
        {
            Collided += OnCollided;
        }

        private void OnDisable()
        {
            Collided -= OnCollided;
        }

        private void OnCollided(Collider2D col)
        {
            var projectileSpawnPos = (Vector2) transform.position -
                         _offset * _rigidbody2D.velocity.normalized;
            if(col.GetComponent<IDamageable>() == null)
                CreateAdditionalProjectiles(projectileSpawnPos);
            Disable();
        }

        private void CreateAdditionalProjectiles(Vector2 newPos)
        {
            for (var i = 0; i < _count; i++)
            {
                Vector2 direction =
                    Quaternion.Euler(0, 0, -90 + _projectilesAngle / 2 + _projectilesAngle * i)
                    * -_rigidbody2D.velocity.normalized;
                _bossShooter.Shoot(newPos + direction, direction);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position - _offset * transform.right, .1f);
        }
    }
}