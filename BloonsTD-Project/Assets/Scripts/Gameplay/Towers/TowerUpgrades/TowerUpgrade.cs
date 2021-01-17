using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Gameplay
{
    public abstract class TowerUpgrade
    {
        protected TowerUpgradeProperties TowerUpgradeProperties { get; private set; }

        public string Name => TowerUpgradeProperties.Name;
        public string Description => TowerUpgradeProperties.Description;
        public int Cost => TowerUpgradeProperties.Cost;
        public Sprite Icon => TowerUpgradeProperties.UpgradeIcon;
        public UpgradeType UpgradeType => TowerUpgradeProperties.UpgradeType;
        public bool IsPurchased { get; private set; }
        public virtual void PurchaseUpgrade()
        {
            IsPurchased = true;
            GameController.Instance.DecrementMoney(Cost);
        }
        public static TowerUpgrade GetNewUpgrade(TowerUpgradeProperties upgradeProperties)
        {
            var upgradeType = upgradeProperties.UpgradeType;
            switch (upgradeType)
            {
                case UpgradeType.Range:
                    return new RangeUpgrade {TowerUpgradeProperties = upgradeProperties};
                case UpgradeType.Frequency:
                    return new FrequencyUpgrade() {TowerUpgradeProperties = upgradeProperties};
                case UpgradeType.Duration:
                    return new DurationUpgrade() {TowerUpgradeProperties = upgradeProperties};
                case UpgradeType.Weapon:
                    return new WeaponUpgrade {TowerUpgradeProperties = upgradeProperties};
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class RangeUpgrade : TowerUpgrade
    {
        private float RangeValue => TowerUpgradeProperties.UpgradeValue;
        public override void PurchaseUpgrade()
        {
            base.PurchaseUpgrade();
            Debug.Log($"Upgrading Range to: {RangeValue}");
        }
    }

    public class WeaponUpgrade : TowerUpgrade
    {
        private GameObject Weapon => TowerUpgradeProperties.Weapon;
        public override void PurchaseUpgrade()
        {
            base.PurchaseUpgrade();
            Debug.Log($"Changing Weapon to: {Weapon.name}");
        }
    }
    
    public class FrequencyUpgrade : TowerUpgrade
    {
        private float FrequencyValue => TowerUpgradeProperties.UpgradeValue;
        public override void PurchaseUpgrade()
        {
            base.PurchaseUpgrade();
            Debug.Log($"Upgrading Frequency to: {FrequencyValue}");
        }
    }

    public class DurationUpgrade : TowerUpgrade
    {
        private float DurationValue => TowerUpgradeProperties.UpgradeValue;
        public override void PurchaseUpgrade()
        {
            base.PurchaseUpgrade();
            Debug.Log($"Upgrading Duration to: {DurationValue}");
        }
    }
}