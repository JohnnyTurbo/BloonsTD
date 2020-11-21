using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    public class TowerStatistics : ScriptableObject
    {
        private float _range;
        private float _attackFrequency;
        private int _numPopped;

        //TODO: change to collections
        private TowerUpgrade _towerUpgradePath1;
        private TowerUpgrade _towerUpgradePath2;

        public float Range => _range;
        public float AttackFrequency => _attackFrequency;
        public int NumPopped => _numPopped;
        public TowerUpgrade TowerUpgradePath1 => _towerUpgradePath1;
        public TowerUpgrade TowerUpgradePath2 => _towerUpgradePath2;
        
        public void Set(TowerProperties properties)
        {
            _towerUpgradePath1 = properties.TowerUpgradePath1;
            _towerUpgradePath2 = properties.TowerUpgradePath2;
            _range = properties.Range;
            _attackFrequency = properties.AttackFrequency;
            _numPopped = 0;
        }
    }
}