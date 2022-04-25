using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    public class PlayerAnimator : ICharacterAnimator
    {
        private readonly int _movementHash = Animator.StringToHash("Movement");
        private readonly Animator _animator;

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void OnMoved(Vector2 offset)
        {
            _animator.SetFloat(_movementHash, offset.sqrMagnitude);
        }
    }
}