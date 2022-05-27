using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using UnityEngine;
using Zenject;

public class Shopkeeper : SerializedMonoBehaviour
{
    [SerializeField]
    private GameObject _shopTable;
    [SerializeField]
    private Dictionary<UpgradeType, Tuple<Currency, int>[]> _priceList = new();
    [SerializeField]
    private Dictionary<UpgradeType, int> _upgradeList = new();

    private PlayerWallet _playerWallet;
    private IUpgradeable _client;

    [Inject]
    private void Init(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }
    
    public void BuyUpgrade(int upgradeType) => BuyUpgrade((UpgradeType) upgradeType);

    private void BuyUpgrade(UpgradeType upgradeType)
    {
        var product = _priceList[upgradeType];
        if (_playerWallet.Pay(product))
            Upgrade(upgradeType);
    }

    private void Upgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.HEALTH:
                _client.UpgradeHealth(_upgradeList[upgradeType]);
                break;
            case UpgradeType.MOVESPEED:
                break;
            case UpgradeType.ATTACKSPEED:
                break;
            case UpgradeType.ATTACKDAMAGE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out IUpgradeable upgradeable)) return;
        _client = upgradeable;
        _shopTable.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IUpgradeable _)) return;
        _client = null;
        _shopTable.gameObject.SetActive(false);
    }
}