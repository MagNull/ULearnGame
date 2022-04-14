using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Projectile : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> BecameUnused;
        [SerializeField]
        private int _damage;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private readonly int _destroyHash = Animator.StringToHash("Destroy");

        public void SetVelocity(Vector2 velocity) => _rigidbody2D.velocity = velocity;

        public void Enable() => gameObject.SetActive(true);

        public void Disable()
        {
            gameObject.SetActive(false);
            BecameUnused?.Invoke(this);
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);
            SetVelocity(Vector2.zero);
            _animator.SetTrigger(_destroyHash);
        }
    }
}