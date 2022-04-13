namespace Sources.Runtime.Interfaces
{
    public interface IFactory<T>
    {
        T Create();
    }
}