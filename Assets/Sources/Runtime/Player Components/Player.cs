using System;
using System.Collections.Generic;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Shop;
using Sources.Runtime.UI___HUD;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Player_Components
{
    public class Player : MonoBehaviour, IDamageable, IShopClient
    {
        public event Action<int> Damaged;
        [SerializeReference]
        private IMovement _movement;
        [SerializeReference]
        private IShooter _shooter;
        [SerializeField]
        private Health _health;
        private PlayerWallet _playerWallet;
        private PlayerAnimator _animator;
        private StopScreen _dieScreen;

        [Inject]
        private void Init(IMovement movement, IShooter shooter, PlayerAnimator animator,
            [Inject(Id = "Player")] Health health, [Inject(Id = "Die Screen")] StopScreen dieScreen, 
            PlayerWallet playerWallet)
        {
            _movement = movement;
            _shooter = shooter;
            _health = health;
            _animator = animator;
            _dieScreen = dieScreen;
            _dieScreen.gameObject.SetActive(false);
            _playerWallet = playerWallet;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            Damaged?.Invoke(_health.Value);
        }

        public bool Pay(Tuple<Currency, int>[] price) => _playerWallet.Pay(price);

        public void AddCurrency(Currency currencyName, int count) => _playerWallet.AddCurrency(currencyName, count);

        public void UpgradeHealth(int value)
        {
            _health.IncreaseHealthValue(value);
            PlayerPrefs.SetInt(PlayerPrefsConstants.PLAYER_HEALTH, _health.Value);
        }

        public void UpgradeMoveSpeed(int value)
        {
            _movement.IncreaseSpeed(value);
            PlayerPrefs.SetFloat(PlayerPrefsConstants.PLAYER_MOVE_SPEED, _movement.Speed);
        }

        public void UpgradeAttackDamage(int value)
        {
            _shooter.IncreaseAttackDamage(value);
            PlayerPrefs.SetInt(PlayerPrefsConstants.PLAYER_DAMAGE, _shooter.ProjectileDamage);
        }

        public void UpgradeAttackSpeed(int value)
        {
            _shooter.IncreaseAttackSpeed(value);
            PlayerPrefs.SetFloat(PlayerPrefsConstants.PLAYER_ATTACK_SPEED, _shooter.AttackSpeed);
        }

        private void OnBalanceChanged(Currency currency, int value)
        {
            switch (currency)
            {
                case Currency.COIN:
                    PlayerPrefs.SetInt(PlayerPrefsConstants.PLAYER_COIN_BALANCE, value);
                    break;
                case Currency.GOLEMHEART:
                    PlayerPrefs.SetInt(PlayerPrefsConstants.PLAYER_GOLEM_HEART_BALANCE, value);
                    break;
                case Currency.REAPERSHARD:
                    PlayerPrefs.SetInt(PlayerPrefsConstants.PLAYER_REAPER_SHARD_BALANCE, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown currency");
            }
        }

        private void OnDied()
        {
            _dieScreen.Enable();
        }

        private void OnEnable()
        {
            _movement.Moved += _animator.OnMoved;
            _health.Died += OnDied;
            _playerWallet.BalanceChanged += OnBalanceChanged;
        }

        private void OnDisable()
        {
            _movement.Moved -= _animator.OnMoved;
            _health.Died -= OnDied;
            _playerWallet.BalanceChanged -= OnBalanceChanged;
        }
    }
}