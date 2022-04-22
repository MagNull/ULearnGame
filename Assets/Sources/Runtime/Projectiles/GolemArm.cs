using Sources.Runtime.Interfaces;
using Sources.Runtime.Utils;
using UnityEngine;

namespace Sources.Runtime
{
    public class GolemArm : Projectile
    {
        private ObjectPool<Projectile> _pool;
        [Header("Additional projectile")]
        [SerializeField]
        private float _offset = 1;
        [SerializeField]
        private int _count = 1;
        [SerializeField]
        private float _speed = 5;

        private float _projectilesAngle;

        public void Init(ObjectPool<Projectile> pool)
        {
            _pool = pool;
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
            var newPos = (Vector2) transform.position -
                         _offset * _rigidbody2D.velocity * Time.deltaTime;
            if(col.GetComponent<IDamageable>() == null)
                CreateAdditionalProjectiles(newPos);
            Disable();
        }

        private void CreateAdditionalProjectiles(Vector2 newPos)
        {
            for (var i = 0; i < _count; i++)
            {
                var projectile = _pool.Get();
                Vector2 direction =
                    Quaternion.Euler(0, 0, -90 + _projectilesAngle / 2 + _projectilesAngle * i)
                    * -_rigidbody2D.velocity.normalized;
                projectile.transform.position = newPos;
                projectile.SetVelocity(_speed * direction);
            }
        }
    }
}