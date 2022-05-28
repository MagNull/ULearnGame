using System;
using System.Collections.Generic;
using Sources.Runtime.Interfaces;

namespace Sources.Runtime.Shop
{
    public interface IShopClient : IUpgradeable
    {
        public event Action<IReadOnlyDictionary<Currency, int>> Paid;
        public bool Pay(Tuple<Currency, int>[] price);
    }
}