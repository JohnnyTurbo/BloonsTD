using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Gameplay
{
    public abstract class TowerUpgrade
    {
        protected TowerUpgradeProperties TowerUpgradeProperties { get; set; }
        public string Name => TowerUpgradeProperties.Name;
        public string Description => TowerUpgradeProperties.Description;
        public UpgradeType UpgradeType => TowerUpgradeProperties.UpgradeType;
        public int Cost => TowerUpgradeProperties.Cost;
        protected float UpgradeValue => TowerUpgradeProperties.UpgradeValue;
        public GameObject Weapon => TowerUpgradeProperties.Weapon;
        public bool HasPurchased { get; private set; }
        public virtual void PurchaseUpgrade()
        {
            Debug.Log("Base Purchase Upgrade");
            
            HasPurchased = true;
        }
        public TowerUpgrade(){}
    }
    
    public class RangeUpgrade : TowerUpgrade
    {
        public UpgradeType UpgradeType => UpgradeType.Range;
        public float RangeValue => UpgradeValue;
        public override void PurchaseUpgrade()
        {
            base.PurchaseUpgrade();
            Debug.Log($"increasing range to: {RangeValue}");
        }

        public RangeUpgrade(TowerUpgradeProperties properties)
        {
            TowerUpgradeProperties = properties;
        }
    }
}