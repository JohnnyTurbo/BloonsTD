using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Gameplay
{
    public abstract class TowerUpgrade
    {
        public TowerUpgradeProperties TowerUpgradeProperties { get; set; }
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
        protected TowerUpgrade(){}
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
}