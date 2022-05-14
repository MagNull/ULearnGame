using UnityEngine;

namespace Sources.Runtime
{
    public class SimpleProjectile : Projectile
    {
        private void OnEnable()
        {
            Collided += OnCollided;
        }

        private void OnDisable()
        {
            Collided -= OnCollided;
        }

        private void OnCollided(Collider2D col)
        {
            SetVelocity(Vector2.zero);
        }
    }
}