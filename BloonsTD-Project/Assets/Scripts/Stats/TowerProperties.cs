using System;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    public enum TowerSpeed
    {
        Slow,
        Medium,
        Fast,
        Hypersonic
    };
    
    [CreateAssetMenu(fileName = "TowerProperties", menuName = "Scriptable Objects/Tower Properties", order = 0)]
    public class TowerProperties : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _cost;
        [SerializeField] private TowerSpeed _speed;
        [SerializeField] private float _attackFrequency;
        [TextArea][SerializeField] private string _description;
        [SerializeField] private float _colliderRadius;
        [SerializeField] private float _range;
        [SerializeField] private bool _canBePlacedOffPath;
        [SerializeField] private bool _canBePlacedOnPath;
        [SerializeField] private TowerUpgrade _towerUpgradePath1;
        [SerializeField] private TowerUpgrade _towerUpgradePath2;
        
        public string Name => _name;
        public int Cost => _cost;
        public string Speed
        {
            get
            {
                switch (_speed)
                {
                    case TowerSpeed.Slow:
                        return "Slow";

                    case TowerSpeed.Medium:
                        return "Medium";

                    case TowerSpeed.Fast:
                        return "Fast";

                    case TowerSpeed.Hypersonic:
                        return "Hypersonic";

                    default:
                        return "Undefined";
                }
            }
        }
        public float AttackFrequency => _attackFrequency;
        public string Description => _description;
        public float ColliderRadius
        {
            get => _colliderRadius;
            set => _colliderRadius = Mathf.Max(0.0001f, value);
        }
        public float Range => _range;
        public bool CanBePlacedOffPath => _canBePlacedOffPath;
        public bool CanBePlacedOnPath => _canBePlacedOnPath;
        public TowerUpgrade TowerUpgradePath1 => _towerUpgradePath1;
        public TowerUpgrade TowerUpgradePath2 => _towerUpgradePath2;
        
        private void OnValidate()
        {
            ColliderRadius = _colliderRadius;
        }
    }
}