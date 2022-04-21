namespace Sources.Runtime.Interfaces
{
    public interface ICharacter : IDamageable
    {
        void Init(params object[] components);
    }
}