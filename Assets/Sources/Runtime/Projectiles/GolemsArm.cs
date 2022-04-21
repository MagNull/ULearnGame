using System;
using Sources.Runtime.Utils;
using UnityEngine;

namespace Sources.Runtime
{
    public class GolemsArm : Projectile
    {
        public ObjectPool<SimpleProjectile> Pool;
        [Header("Additional projectile")]
        [SerializeField]
        private float _offset = 1;
        [SerializeField]
        private int _count = 1;
        [SerializeField]
        private float _speed = 5;

        private float _projectilesAngle;

        private void Start()
        {
            _projectilesAngle = Mathf.PI / _count * Mathf.Rad2Deg;
        }

        protected override void OnCollided()
        {
            var newPos = (Vector2) transform.position -
                         _offset * _rigidbody2D.velocity * Time.deltaTime;

            CreateAdditionalProjectiles(newPos);
            Disable();
        }

        private void CreateAdditionalProjectiles(Vector2 newPos)
        {
            for (var i = 0; i < _count; i++)
            {
                var projectile = Pool.Get();
                Vector2 offset =
                    Quaternion.Euler(0, 0, -90 + _projectilesAngle / 2 + _projectilesAngle * i)
                    * -_rigidbody2D.velocity.normalized;
                projectile.transform.position = newPos + offset;
                projectile.SetVelocity(_speed * offset);
            }
        }
    }
}