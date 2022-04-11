using System;
using UnityEngine;

namespace Sources.Runtime
{
    [Serializable]
    public class PlayerMovement : IMovement
    {
        [SerializeField] private float _speed;
        private readonly Rigidbody2D _rigidbody2D;

        public PlayerMovement(Rigidbody2D rigidbody2D, float speed)
        {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
        }

        public void Move(Vector2 movement) =>
            _rigidbody2D.MovePosition((Vector2) _rigidbody2D.transform.position + movement * _speed * Time.deltaTime);
    }
}