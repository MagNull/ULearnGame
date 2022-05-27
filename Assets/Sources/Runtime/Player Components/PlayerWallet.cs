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
        [SerializeField]
        private Dictionary<Currency, int> _wallet;

        public PlayerWallet(Dictionary<Currency, int> wallet)
        {
            _wallet = wallet;
        }
        
        public bool Pay(Tuple<Currency, int>[] price)
        {
            if (price
                .Any(pricePair =>
                    !_wallet.ContainsKey(pricePair.Item1) || _wallet[pricePair.Item1] < pricePair.Item2))
            {
                Debug.Log("Not Pay");
                return false;
            }

            Debug.Log("Pay");
            price.ForEach(pricePair => _wallet[pricePair.Item1] -= pricePair.Item2);
            return true;
        }

        public void AddCurrency(Currency currencyName, int count)
        {
            if (_wallet.ContainsKey(currencyName))
                _wallet[currencyName] += count;
            else
                _wallet.Add(currencyName, count);
        }
    }
}