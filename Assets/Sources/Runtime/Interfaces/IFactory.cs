namespace Sources.Runtime.Interfaces
{
    public interface IFactory<in TProduct>
    {
        TConcreteProduct Create<TConcreteProduct, TProductOwner>() where TConcreteProduct : TProduct;
    }
}