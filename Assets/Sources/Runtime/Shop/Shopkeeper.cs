using System;
using System.Collections.Generic;
using Sources.Runtime.Shop;

public class Shopkeeper
{
    private readonly Dictionary<UpgradeType, Tuple<Currency, int>[]> _priceList;
    private readonly Dictionary<UpgradeType, int> _upgradeList;

    public IShopClient Client { get; set; }

    public Shopkeeper(Dictionary<UpgradeType, Tuple<Currency, int>[]> priceList,
        Dictionary<UpgradeType, int> upgradeList)
    {
        _priceList = priceList;
        _upgradeList = upgradeList;
    }


    public void BuyUpgrade(UpgradeType upgradeType)
    {
        var product = _priceList[upgradeType];
        if (Client.Pay(product))
            Upgrade(upgradeType);
    }

    private void Upgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.HEALTH:
                Client.UpgradeHealth(_upgradeList[upgradeType]);
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
}