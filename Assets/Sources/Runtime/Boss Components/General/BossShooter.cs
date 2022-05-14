using System;
using System.Collections;
using Sources.Runtime.Utils;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components
{
    public abstract class BossShooter : MonoBehaviour
    {
        [SerializeField]
        private float _projectileSpeed = 5;
        [SerializeField]
        private int _projectileBuffer = 100;
        private ObjectPool<Projectile> _projectilePool;

        [Inject]
        public void Init(ProjectileFactory projectileFactory)
        {
            _projectilePool =
                new ObjectPool<Projectile>(_projectileBuffer, projectileFactory.Create<Projectile, Boss>);
        }

        public void Shoot(Vector3 start, Vector2 direction, Projectile projectile = null, float delay = 0)
        {
            Action shot = () =>
            {
                if (projectile == null)
                    projectile = _projectilePool.Get();
                projectile.transform.position = start;
                projectile.SetVelocity(_projectileSpeed * direction);
            };

            if (delay > 0) StartCoroutine(Delay(delay, shot));
            else shot.Invoke();
        }

        private IEnumerator Delay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        public Projectile CreateProjectile() => _projectilePool.Get();
    }
}