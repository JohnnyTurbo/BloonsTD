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
        public virtual void PurchaseUpgrade(TowerController towerController)
        {
            IsPurchased = true;
            GameController.Instance.DecrementMoney(Cost);
            towerController.IncreaseTotalTowerCost(Cost);
        }
        public static TowerUpgrade GetNewUpgrade(TowerUpgradeProperties upgradeProperties)
        {
            if (upgradeProperties == null) {return null;}
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
        public override void PurchaseUpgrade(TowerController towerController)
        {
            if (towerController.TowerAttack is IUpgradeRange rangeUpgrade)
            {
                rangeUpgrade.SetRange(RangeValue);
                base.PurchaseUpgrade(towerController);
            }
            else
            {
                Debug.LogWarning("Warning: TowerAttack does not implement IUpgradeRange interface.",
                    towerController.gameObject);
            }
        }
    }

    public class WeaponUpgrade : TowerUpgrade
    {
        private GameObject Weapon => TowerUpgradeProperties.Weapon;
        public override void PurchaseUpgrade(TowerController towerController)
        {
            if (towerController.TowerAttack is IUpgradeWeapon weaponUpgrade)
            {
                base.PurchaseUpgrade(towerController);
                weaponUpgrade.SetWeapon(Weapon);
            }
            else
            {
                Debug.LogWarning("Warning: TowerAttack does not implement IUpgradeWeapon interface.",
                    towerController.gameObject);
            }
        }
    }
    
    public class FrequencyUpgrade : TowerUpgrade
    {
        private float FrequencyValue => TowerUpgradeProperties.UpgradeValue;
        public override void PurchaseUpgrade(TowerController towerController)
        {
            if (towerController.TowerAttack is IUpgradeFrequency frequencyUpgrade)
            {
                base.PurchaseUpgrade(towerController);
                frequencyUpgrade.SetFrequency(FrequencyValue);
            }
            else
            {
                Debug.LogWarning("Warning: TowerAttack does not implement IUpgradeFrequency interface.",
                    towerController.gameObject);
            }
        }
    }

    public class DurationUpgrade : TowerUpgrade
    {
        private float DurationValue => TowerUpgradeProperties.UpgradeValue;
        public override void PurchaseUpgrade(TowerController towerController)
        {
            if (towerController.TowerAttack is IUpgradeDuration durationUpgrade)
            {
                base.PurchaseUpgrade(towerController);
                durationUpgrade.SetDuration(DurationValue);
            }
            else
            {
                Debug.LogWarning("Warning: TowerAttack does not implement IUpgradeDuration interface.",
                    towerController.gameObject);
            }
        }
    }
}