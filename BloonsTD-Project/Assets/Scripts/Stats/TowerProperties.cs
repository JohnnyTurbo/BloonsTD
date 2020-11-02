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
    
    [CreateAssetMenu(fileName = "TowerProperties", menuName = "ScriptableObjects/Tower Properties", order = 0)]
    public class TowerProperties : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _cost;
        [SerializeField] private TowerSpeed _speed;
        [TextArea][SerializeField] private string _description;
        [SerializeField] private float _colliderRadius;
        [SerializeField] private bool _canBePlacedOffPath;
        [SerializeField] private bool _canBePlacedOnPath;
        
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

        public string Description => _description;
        public float ColliderRadius
        {
            get => _colliderRadius;
            set => _colliderRadius = Mathf.Max(0.0001f, value);
        }

        public bool CanBePlacedOffPath => _canBePlacedOffPath;

        public bool CanBePlacedOnPath => _canBePlacedOnPath;

        private void OnValidate()
        {
            ColliderRadius = _colliderRadius;
        }
    }
}