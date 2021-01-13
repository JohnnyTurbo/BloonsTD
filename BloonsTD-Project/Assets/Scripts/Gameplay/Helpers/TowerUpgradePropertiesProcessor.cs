using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    /*public static class TowerUpgradeProcessor
    {
        private static Dictionary<UpgradeType, TowerUpgrade> _upgradeProperties =
            new Dictionary<UpgradeType, TowerUpgrade>();
        private static bool _initialized;

        private static void Initialize()
        {
            _upgradeProperties.Clear();
            if (Resources.FindObjectsOfTypeAll(typeof(TowerUpgrade)) is TowerUpgrade[]  allTowerUpgradeProperties)
            {
                foreach (var towerUpgradeProperty in allTowerUpgradeProperties)
                {
                    _upgradeProperties.Add(towerUpgradeProperty.UpgradeType, towerUpgradeProperty);
                }

                _initialized = true;
            }
        }

        public static TowerUpgrade GetTowerUpgradePropertiesFromUpgradeType(UpgradeType upgradeType)
        {
            if(!_initialized){Initialize();}

            return _upgradeProperties[upgradeType];
        }
    }*/
}