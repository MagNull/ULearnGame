using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    [Serializable]
    public class PlayerMovement : IMovement
    {
        public event Action<Vector2> Moved;
        [SerializeField] private float _speed;
        private readonly Rigidbody2D _rigidbody2D;

        public PlayerMovement(Rigidbody2D rigidbody2D, float speed)
        {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
        }

        public void Move(Vector2 movement)
        {
            _rigidbody2D.MovePosition((Vector2) _rigidbody2D.transform.position + movement * _speed * Time.deltaTime);
            LookAtMovement(movement);
            Moved?.Invoke(movement);
        }

        private void LookAtMovement(Vector2 movement)
        {
            if(movement.sqrMagnitude == 0)
                return;
            var newRotation = Mathf.Sign(movement.x) > 0 ? Vector3.zero : Vector3.up * 180;
            _rigidbody2D.transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}