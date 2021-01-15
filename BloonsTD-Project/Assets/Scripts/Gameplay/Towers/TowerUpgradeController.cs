using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerUpgradeController : MonoBehaviour
    {
        public delegate void UpgradeTower(TowerUpgrade towerUpgrade);
        public event UpgradeTower OnUpgradeTower;
        public TowerController TowerController { get; private set; }
        public TowerUpgrade[] Upgrades => _upgrades;
        private TowerUpgrade[] _upgrades = new TowerUpgrade[2];

        public void InitializeUpgrades(TowerController controller)
        {
            TowerController = controller;
            var towerProperties = controller.TowerProperties;
            _upgrades[0] = new WeaponUpgrade() {TowerUpgradeProperties = towerProperties.TowerUpgradePropertiesPath1};
            _upgrades[1] = new RangeUpgrade() {TowerUpgradeProperties = towerProperties.TowerUpgradePropertiesPath2};
        }
    }
}