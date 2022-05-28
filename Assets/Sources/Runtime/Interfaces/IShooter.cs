namespace Sources.Runtime.Interfaces
{
    public interface IShooter
    {
        public float AttackSpeed { get; }

        public int ProjectileDamage { get; }
        void Shoot();
        void IncreaseAttackSpeed(float value);
        void IncreaseAttackDamage(int value);
    }
}