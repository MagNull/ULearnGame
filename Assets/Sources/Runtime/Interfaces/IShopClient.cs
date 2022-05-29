using System;
using System.Collections.Generic;
using Sources.Runtime.Interfaces;

namespace Sources.Runtime.Shop
{
    public interface IShopClient : IUpgradeable
    {
        public bool Pay(Tuple<Currency, int>[] price);
    }
}