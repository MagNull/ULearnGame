using UnityEngine;

namespace Sources.Runtime
{
    public interface ICharacterAnimator
    {
        void OnMoved(Vector2 offset);
    }
}