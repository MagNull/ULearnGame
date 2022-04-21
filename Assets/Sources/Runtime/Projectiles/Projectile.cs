using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Projectile : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        [SerializeField]
        private int _damage;
        protected Rigidbody2D _rigidbody2D;

        public void SetVelocity(Vector2 velocity) => _rigidbody2D.velocity = velocity;

        public void Enable() => gameObject.SetActive(true);

        public void Disable()
        {
            gameObject.SetActive(false);
            BecameUnused?.Invoke(this);
        }

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);
            OnCollided();
            SetVelocity(Vector2.zero);
        }

        protected abstract void OnCollided();
    }
}