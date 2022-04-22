using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Projectile : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        public event Action<Collider2D> Collided;
        [SerializeField]
        private int _damage;
        protected Rigidbody2D _rigidbody2D;

        public void Init(Rigidbody2D rigidbody2D, ProjectileAnimator animator)
        {
            _rigidbody2D = rigidbody2D;
            Collided += animator.OnCollided;
        }
        
        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody2D.velocity = velocity;
            LookAtVelocity();
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

        public void Enable() => gameObject.SetActive(true);

        public void Disable()
        {
            gameObject.SetActive(false);
            BecameUnused?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);
            Collided?.Invoke(col);
            SetVelocity(Vector2.zero);
        }
    }
}