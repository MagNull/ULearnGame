using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Boss_Components.Reaper
{
    public class ReaperAttack : BossAttack<ReaperShooter>
    {
        [SerializeField]
        private Transform _rotationTarget;
        
        [Header("Scythe Attack")]
        [SerializeField]
        private int _SAProjectilesCount;
        [SerializeField]
        private float _creationDelay;
        [SerializeField]
        private float _shootDelay;
        [SerializeField]
        private int _SAAngle;
        [SerializeField]
        private Transform _upAttackPos;
        [SerializeField]
        private Transform _downAttackPos;
        private readonly List<(Projectile, Vector3)> _chargedProjectiles = new();

        private Transform _playerTransform;
        private bool _isStatic = false;

        [Inject]
        public void Init(BossPhaseSwitching bossPhaseSwitching, BossAnimator animator, ReaperShooter shooter,
            [Inject(Id = "Player")] Transform playerTransform)
        {
            base.Init(bossPhaseSwitching, animator, shooter);
            _playerTransform = playerTransform;
        }

        public void StartScytheAttack()
        {
            _isStatic = true;
            StartCoroutine(ScytheAttackCreation());
        }

        private IEnumerator ScytheAttackCreation()
        {
            var deltaAngle = _SAAngle / _SAProjectilesCount;
            var positionVector = _upAttackPos.position - transform.position;
            for (var i = 0; i < _SAProjectilesCount; i++)
            {
                var projectile = _shooter.CreateProjectile();
                var direction = 
                    Quaternion.AngleAxis(deltaAngle * -i, transform.forward) * positionVector;
                projectile.transform.position = transform.position + direction;
                _chargedProjectiles.Add((projectile, direction));
                yield return new WaitForSeconds(_creationDelay);
            }
        }

        private void PushScytheAttack()
        {
            StopAllCoroutines();
            for (var i = 0; i < _chargedProjectiles.Count; i++)
            {
                _shooter.Shoot(transform.position + _chargedProjectiles[i].Item2, _chargedProjectiles[i].Item2,
                    _chargedProjectiles[i].Item1, _shootDelay * i);
            }

            _isStatic = false;
            _chargedProjectiles.Clear();
        }

        private void Update()
        {
            if (!_isStatic)
                LookAtPlayerSide();
        }

        private void LookAtPlayerSide()
        {
            var playerDirection = _playerTransform.transform.position - transform.position;
            _rotationTarget.right = new Vector3(playerDirection.normalized.x, 0);
        }

        protected override void IncreaseAttackSpeed()
        {
            throw new System.NotImplementedException();
        }
    }
}