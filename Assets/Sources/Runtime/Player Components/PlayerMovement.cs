using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    [Serializable]
    public class PlayerMovement : IMovement
    {
        public event Action<Vector2> Moved;
        [SerializeField]
        private float _speed;
        private readonly Transform _rotationTarget;
        private readonly Rigidbody2D _rigidbody2D;

        public float Speed => _speed;

        public PlayerMovement(Rigidbody2D rigidbody2D, float speed, Transform rotationTarget)
        {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
            _rotationTarget = rotationTarget;
        }

        public void Move(Vector2 direction)
        {
            if (direction.sqrMagnitude > 0)
            {
                _rigidbody2D.MovePosition((Vector2) _rigidbody2D.transform.position +
                                          direction * _speed * Time.deltaTime);
                LookAtMovement(direction);
            }

            Moved?.Invoke(direction);
        }

        public void IncreaseSpeed(float value) => _speed += value * (_speed / 100);

        private void LookAtMovement(Vector2 movement)
        {
            if (movement.sqrMagnitude == 0)
                return;
            var newRotation = Mathf.Sign(movement.x) > 0 ? Vector3.zero : Vector3.up * 180;
            _rotationTarget.rotation = Quaternion.Euler(newRotation);
        }
    }
}