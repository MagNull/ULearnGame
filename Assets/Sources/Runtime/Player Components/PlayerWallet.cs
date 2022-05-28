using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    [Serializable]
    public class PlayerWallet
    {
        public IReadOnlyDictionary<Currency, int> WalletBalance => _walletBalance;
        
        [SerializeField]
        private readonly Dictionary<Currency, int> _walletBalance;

        public PlayerWallet(Dictionary<Currency, int> walletBalance)
        {
            _walletBalance = walletBalance;
        }
        
        public bool Pay(Tuple<Currency, int>[] price)
        {
            if (price
                .Any(pricePair =>
                    !_walletBalance.ContainsKey(pricePair.Item1) || _walletBalance[pricePair.Item1] < pricePair.Item2))
            {
                Debug.Log("Not Pay");
                return false;
            }

            Debug.Log("Pay");
            price.ForEach(pricePair => _walletBalance[pricePair.Item1] -= pricePair.Item2);
            return true;
        }

        public void AddCurrency(Currency currencyName, int count)
        {
            if (_walletBalance.ContainsKey(currencyName))
                _walletBalance[currencyName] += count;
            else
                _walletBalance.Add(currencyName, count);
        }
    }
}