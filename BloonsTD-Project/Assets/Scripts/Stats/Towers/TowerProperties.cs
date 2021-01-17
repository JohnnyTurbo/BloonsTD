using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "TowerProperties", menuName = "Scriptable Objects/Tower Properties", order = 0)]
    public class TowerProperties : ScriptableObject
    {
        [SerializeField] private string _towerName;
        [SerializeField] private int _cost;
        [SerializeField] private TowerSpeed _speed;
        [SerializeField] private float _attackCooldownTime;
        [TextArea][SerializeField] private string _description;
        [SerializeField] private float _colliderRadius;
        [SerializeField] private float _range;
        [SerializeField] private bool _canBePlacedOffPath;
        [SerializeField] private bool _canBePlacedOnPath;
        [SerializeField] private TowerAttackType _towerAttackType;
        //TODO: in custom editor display either projectile prefab or status duration based on attack type
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private int _numberProjectiles;
        [SerializeField] private float _freezeDuration;

        [SerializeField] private TowerUpgradeProperties _towerUpgradePropertiesPath1;
        [SerializeField] private TowerUpgradeProperties _towerUpgradePropertiesPath2;
        
        public string TowerName => _towerName;
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
        public float AttackCooldownTime => _attackCooldownTime;
        public string Description => _description;
        public float ColliderRadius
        {
            get => _colliderRadius;
            private set => _colliderRadius = Mathf.Max(0.0001f, value);
        }
        public float Range => _range;
        public bool CanBePlacedOffPath => _canBePlacedOffPath;
        public bool CanBePlacedOnPath => _canBePlacedOnPath;
        public TowerAttackType TowerAttackType => _towerAttackType;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public int NumberProjectiles => _numberProjectiles;
        public float FreezeDuration => _freezeDuration;
        public TowerUpgradeProperties TowerUpgradePropertiesPath1 => _towerUpgradePropertiesPath1;
        public TowerUpgradeProperties TowerUpgradePropertiesPath2 => _towerUpgradePropertiesPath2;
        
        private void OnValidate()
        {
            ColliderRadius = _colliderRadius;
        }
    }
}