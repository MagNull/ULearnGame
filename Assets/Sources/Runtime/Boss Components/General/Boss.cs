using System;
using System.Collections.Generic;
using DG.Tweening;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using Sources.Runtime.Utils;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Runtime.Boss_Components
{
    public class Boss : MonoBehaviour, IDamageable
    {
        public event Action<int> Damaged;
        [SerializeField]
        private Health _health;
        [Header("Drop configs")]
        [SerializeField]
        private UnitySerializedDictionary<CurrencyItem, int> _dropList;
        [SerializeField]
        private float _dropShootPower;
        [SerializeField]
        private int _maxDropJumps;
        [SerializeField]
        private float _dropJumpDuration;
        [SerializeField]
        private float _dropShootDistance;
        
        private BossAnimator _animator;
        private ProjectileFactory _projectileFactory;

        public Health Health => _health;

        [Inject]
        protected void Init(BossAnimator animator, ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
            _animator = animator;
            enabled = true;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            Damaged?.Invoke(_health.Value);
        }

        private void OnEnable()
        {
            _health.Died += _animator.OnDied;
        }

        private void OnDisable()
        {
            _health.Died -= _animator.OnDied;
        }

        private void OnDied()
        {
            ShootDrop();
            _projectileFactory.DestroyAllProjectiles();
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }

        private void ShootDrop()
        {
            foreach (var drop in _dropList)
            {
                for (var i = 0; i < drop.Value; i++)
                {
                    var currencyItem = Instantiate(drop.Key, transform.position, Quaternion.identity);
                    var randomPos = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.up *
                                    _dropShootDistance;
                    var jumpCount = Random.Range(1, _maxDropJumps + 1);
                    currencyItem.transform.DOJump(transform.position + randomPos, _dropShootPower,
                        jumpCount, _dropJumpDuration * jumpCount);
                }
            }
        }
    }
}