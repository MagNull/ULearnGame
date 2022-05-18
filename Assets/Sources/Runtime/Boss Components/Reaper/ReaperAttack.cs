using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
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
        private float _SAAngle;
        [SerializeField]
        private Transform _upAttackPos;
        [SerializeField]
        private Transform _downAttackPos;

        [Header("Summon Attack")]
        [SerializeField]
        private int _summonCount = 4;

        [Header("Scythe Damage Zone")]
        [SerializeField]
        private Transform _damageZonePos;
        [SerializeField]
        private float _damageZoneRadius;
        [SerializeField]
        private LayerMask _damageLayerMask;
        [SerializeField]
        private int _scytheDamage;

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

        public void StartScytheAttack(string direction)
        {
            StartCoroutine(ScytheAttackCreation(direction.ToLower() == "up" ? _upAttackPos : _downAttackPos));
        }

        public void SummonAttack()
        {
            for (int i = 0; i < _summonCount; i++)
            {
                _shooter.ShootSummon();
            }
        }

        private IEnumerator ScytheAttackCreation(Transform start)
        {
            var deltaAngle = _SAAngle / _SAProjectilesCount;
            var positionVector = start.position - transform.position;
            for (var i = 0; i < _SAProjectilesCount; i++)
            {
                var projectile = _shooter.CreateProjectile();
                var direction =
                    Quaternion.AngleAxis(deltaAngle * -Mathf.Sign(start.localPosition.y) * i, transform.forward) * positionVector;
                projectile.transform.position = transform.position + direction;
                _chargedProjectiles.Add((projectile, direction));
                yield return new WaitForSeconds(_creationDelay);
            }
        }

        private void SwungScythe()
        {
            StopAllCoroutines();
            _shooter.ShootChargedProjectiles(_chargedProjectiles, _shootDelay);
            DamageSwungZone();
            _chargedProjectiles.Clear();
        }

        private void DamageSwungZone()
        {
            var result = 
                Physics2D.OverlapCircle(_damageZonePos.position, _damageZoneRadius, _damageLayerMask);
            if (result && result.TryGetComponent(out Player player))
                player.TakeDamage(_scytheDamage);
        }

        private void LookAtPlayerSide()
        {
            var playerDirection = _playerTransform.transform.position - transform.position;
            _rotationTarget.right = new Vector3(playerDirection.normalized.x, 0);
        }

        protected override void IncreaseAttackSpeed()
        {
            _creationDelay /= PhaseSwitchAttackSpeedMulti;
            _shootDelay /= PhaseSwitchAttackSpeedMulti;
            _bossAnimator.IncreaseAttackSpeedMulti(PhaseSwitchAttackSpeedMulti);
        }

        private void Update()
        {
            if (!_isStatic)
                LookAtPlayerSide();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_damageZonePos.position, _damageZoneRadius);
        }
    }
}