using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public abstract class Projectile : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        public event Action<Collider2D> Collided;
        [SerializeField]
        private int _damage;

        [SerializeField]
        private bool _isRotationStatic;
        
        protected Rigidbody2D _rigidbody2D;

        public void Init(ProjectileAnimator animator)
        {
            Collided += animator.OnCollided;
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody2D.velocity = velocity;
            if(!_isRotationStatic)
                LookAtVelocity();
        }

        public void SetDamage(int damage) => _damage = damage;

        public void Enable() => gameObject.SetActive(true);

        public void Disable()
        {
            gameObject.SetActive(false);
            BecameUnused?.Invoke(this);
        }

        private void LookAtVelocity()
        {
            var newRotation =
                Quaternion.LookRotation(Vector3.forward, 
                    Vector3.Cross(Vector3.forward, _rigidbody2D.velocity));
            transform.rotation = newRotation;

            if (transform.up.y < 0) 
                transform.Rotate(new Vector3(180, 0, 0));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);
            Collided?.Invoke(col);
        }
    }
}