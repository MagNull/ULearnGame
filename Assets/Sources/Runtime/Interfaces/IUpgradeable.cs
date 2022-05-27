namespace Sources.Runtime.Interfaces
{
    public interface IUpgradeable
    {
        public void UpgradeHealth(int value);
        public void UpgradeMove(int value);
        public void UpgradeAttackDamage(int value);
        public void UpgradeAttackSpeed(int value);
    }
}