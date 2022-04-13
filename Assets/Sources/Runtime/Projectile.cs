using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        [SerializeField]
        private int _damage;
        private Rigidbody2D _rigidbody2D;

        public void SetVelocity(Vector2 velocity) => _rigidbody2D.velocity = velocity;

        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);
            BecameUnused?.Invoke(this);
        }
    }
}