using System;

namespace Sources.Runtime.Shop
{
    public interface IPayable
    {
        public bool Pay(Tuple<Currency, int>[] price);
    }
}