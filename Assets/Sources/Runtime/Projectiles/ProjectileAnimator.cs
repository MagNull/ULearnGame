using UnityEngine;

namespace Sources.Runtime
{
    public class ProjectileAnimator
    {
        private readonly Animator _animator;
        private readonly int _destroyHash = Animator.StringToHash("Destroy");

        public ProjectileAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void OnCollided(Collider2D col)
        {
            _animator.SetTrigger(_destroyHash);
        }
    }
}