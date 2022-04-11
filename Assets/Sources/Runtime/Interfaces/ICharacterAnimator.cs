using UnityEngine;

namespace Sources.Runtime.Interfaces
{
    public interface ICharacterAnimator
    {
        void OnMoved(Vector2 offset);
    }
}