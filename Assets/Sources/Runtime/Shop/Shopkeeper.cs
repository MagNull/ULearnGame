using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Runtime;
using Sources.Runtime.Shop;
using UnityEngine;

public class Shopkeeper
{
    public event Action<UpgradeType, Tuple<Currency, int>[]> PriceChanged; 
    private readonly Dictionary<UpgradeType, Tuple<Currency, int>[]> _priceList;
    private readonly Dictionary<UpgradeType, int> _upgradeList;
    private readonly Dictionary<UpgradeType, float> _upgradePriceChange;
    private readonly Dictionary<UpgradeType, string[]> _upgradePlayerPrefs = new()
    {
        {UpgradeType.HEALTH, new[] {PlayerPrefsConstants.HEALTH_PRICE1, PlayerPrefsConstants.HEALTH_PRICE2}},
        {UpgradeType.ATTACKDAMAGE, new[] {PlayerPrefsConstants.ATTACK_DAMAGE_PRICE1, PlayerPrefsConstants.ATTACK_DAMAGE_PRICE2}},
        {UpgradeType.MOVESPEED, new[] {PlayerPrefsConstants.MOVE_SPEED_PRICE}},
        {UpgradeType.ATTACKSPEED, new[] {PlayerPrefsConstants.ATTACK_SPEED_PRICE}}
    };

    public IShopClient Client { get; set; }

    public Shopkeeper(Dictionary<UpgradeType, Tuple<Currency, int>[]> priceList,
        Dictionary<UpgradeType, int> upgradeList, Dictionary<UpgradeType, float> upgradePriceChange)
    {
        _priceList = priceList;
        _upgradeList = upgradeList;
        _upgradePriceChange = upgradePriceChange;
    }

    public void Init()
    {
        LoadPrice();
        foreach (var upgradeType in _priceList.Keys)
        {
            PriceChanged?.Invoke(upgradeType, _priceList[upgradeType]);
        }
    }

    public void BuyUpgrade(UpgradeType upgradeType)
    {
        if (!Client.Pay(_priceList[upgradeType])) 
            return;
        
        Upgrade(upgradeType);
        for (var i = 0; i < _priceList[upgradeType].Length; i++)
        {
            var oldPrice = _priceList[upgradeType][i];
            _priceList[upgradeType][i] =
                new Tuple<Currency, int>(oldPrice.Item1,
                    Mathf.CeilToInt(oldPrice.Item2 * _upgradePriceChange[upgradeType]));
            PlayerPrefs.SetInt(_upgradePlayerPrefs[upgradeType][i], _priceList[upgradeType][i].Item2);
        }

        PriceChanged?.Invoke(upgradeType, _priceList[upgradeType]);
    }

    private void LoadPrice()
    {
        foreach (var upgradeType in _priceList.Keys)
        {
            var price = _priceList[upgradeType];
            for (var i = 0; i < price.Length; i++)
            {
                if (PlayerPrefs.GetInt(_upgradePlayerPrefs[upgradeType][i]) == 0)
                    continue;
                var newPrice = new Tuple<Currency, int>(_priceList[upgradeType][i].Item1,
                    PlayerPrefs.GetInt(_upgradePlayerPrefs[upgradeType][i]));
                _priceList[upgradeType][i] = newPrice;
            }
        }
    }

    private void Upgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.HEALTH:
                Client.UpgradeHealth(_upgradeList[upgradeType]);
                break;
            case UpgradeType.MOVESPEED:
                Client.UpgradeMoveSpeed(_upgradeList[upgradeType]);
                break;
            case UpgradeType.ATTACKSPEED:
                Client.UpgradeAttackSpeed(_upgradeList[upgradeType]);
                break;
            case UpgradeType.ATTACKDAMAGE:
                Client.UpgradeAttackDamage(_upgradeList[upgradeType]);
                break;
            default:
                throw new ArgumentOutOfRangeException("Unknown upgrade");
        }
    }
}