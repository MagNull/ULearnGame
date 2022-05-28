namespace Sources.Runtime.Interfaces
{
    public interface IUpgradeable
    {
        public void UpgradeHealth(int value);
        public void UpgradeMoveSpeed(int value);
        public void UpgradeAttackDamage(int value);
        public void UpgradeAttackSpeed(int value);
    }
}