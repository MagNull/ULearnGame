using System;
using System.Collections;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sources.Runtime.Player_Components
{
    [Serializable]
    public class PlayerShooter : IShooter
    {
        public float AttackSpeed => 1 / _shootDelay;
        public int ProjectileDamage => _projectileDamage;

        [SerializeField]
        private float _projectileSpeed;
        [SerializeField]
        private float _shootDelay;
        [SerializeField]
        private int _projectileDamage;

        private readonly ObjectPool<Projectile> _projectilePool;
        private readonly Transform _shootOrigin;
        private readonly Camera _camera;

        private float _lastShootTime = 0;

        public PlayerShooter(ObjectPool<Projectile> projectilePool, Transform shootOrigin, float projectileSpeed,
            float shootDelay, int projectileDamage)
        {
            _projectilePool = projectilePool;
            _shootOrigin = shootOrigin;
            _projectileSpeed = projectileSpeed;
            _shootDelay = shootDelay;
            _projectileDamage = projectileDamage;
            _camera = Camera.main;
        }


        public void Shoot()
        {
            if (Time.time - _lastShootTime < _shootDelay)
                return;

            _lastShootTime = Time.time;
            var projectile = _projectilePool.Get();
            projectile.SetDamage(_projectileDamage);
            projectile.transform.position = _shootOrigin.position;
            Vector2 shootDirection = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) -
                                     _shootOrigin.position;
            projectile.SetVelocity(shootDirection.normalized * _projectileSpeed);
        }

        public void IncreaseAttackSpeed(float value) => _shootDelay -= value * (_shootDelay / 100);
        public void IncreaseAttackDamage(int value) => _projectileDamage += value;

        private IEnumerator ShootingCoroutine()
        {
            while (true)
            {
                if (Time.time - _lastShootTime >= _shootDelay)
                {
                    Shoot();
                    _lastShootTime = Time.time;
                }

                yield return new WaitForSeconds(_shootDelay);
            }
        }
    }
}