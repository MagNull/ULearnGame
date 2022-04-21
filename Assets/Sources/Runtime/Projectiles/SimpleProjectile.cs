using UnityEngine;

namespace Sources.Runtime
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class SimpleProjectile : Projectile
    {
        private Animator _animator;
        private readonly int _destroyHash = Animator.StringToHash("Destroy");

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        protected override void OnCollided()
        {
            _animator.SetTrigger(_destroyHash);
        }
    }
}