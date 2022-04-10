using UnityEngine;

namespace Sources.Runtime
{
    public class PlayerMovement : IMovement
    {
        private readonly Rigidbody2D _rigidbody2D;
        private float _speed;

        public PlayerMovement(Rigidbody2D rigidbody2D, float speed)
        {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
        }

        public void Move(Vector2 movement) =>
            _rigidbody2D.MovePosition((Vector2) _rigidbody2D.transform.position + movement * _speed * Time.deltaTime);
    }
}