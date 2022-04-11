using System;
using UnityEngine;

namespace Sources.Runtime
{
    public interface IMovement
    {
        public event Action<Vector2> Moved;
        
        void Move(Vector2 movement);
    }
}