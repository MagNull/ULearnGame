using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Shop
{
    [ShowOdinSerializedPropertiesInInspector]
    public class ShopkeeperPresenter : SerializedMonoBehaviour
    {
        [Header("Initialize configs")]
        [SerializeField]
        private Dictionary<UpgradeType, Tuple<Currency, int>[]> _priceList;
        [SerializeField]
        private Dictionary<UpgradeType, int> _upgradeList;
        [SerializeField]
        private readonly Dictionary<UpgradeType, float> _upgradePriceChange;

        [Space]
        [Header("View configs")]
        [SerializeField]
        private GameObject _shopTable;
        [SerializeField]
        private Dictionary<Currency, TextMeshProUGUI> _currencyBalanceViews;
        [SerializeField]
        private Dictionary<Currency, string> _currencyNames;
        [SerializeField]
        private Dictionary<UpgradeType, Dictionary<Currency, TextMeshProUGUI>> _pricesView;

        private Shopkeeper _shopkeeper;

        public void BuyUpgrade(int upgradeType) => _shopkeeper.BuyUpgrade((UpgradeType) upgradeType);

        [Inject]
        private void Init(PlayerWallet wallet) //TODO: Change
        {
            OnClientPaid(wallet.WalletBalance);
            _shopkeeper = new Shopkeeper(_priceList, _upgradeList, _upgradePriceChange);
            _shopkeeper.PriceChanged += OnPriceChanged;
            _shopkeeper.Init();
        }

        private void OnClientPaid(IReadOnlyDictionary<Currency, int> wallet)
        {
            foreach (var (currency, priceView) in _currencyBalanceViews)
            {
                if (!wallet.ContainsKey(currency))
                    continue;

                priceView.transform.parent.gameObject.SetActive(true);
                _currencyBalanceViews[currency].text =
                    _currencyNames[currency] + ": " + wallet[currency];
            }
        }

        private void OnPriceChanged(UpgradeType upgradeType, Tuple<Currency, int>[] newPrice)
        {
            newPrice.ForEach(price => 
                _pricesView[upgradeType][price.Item1].text = price.Item2.ToString());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Player client)) return;
            client.Paid += OnClientPaid;
            _shopkeeper.Client = client;
            _shopTable.gameObject.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player client)) return;
            client.Paid -= OnClientPaid;
            _shopkeeper.Client = null;
            _shopTable.gameObject.SetActive(false);
        }
    }
}