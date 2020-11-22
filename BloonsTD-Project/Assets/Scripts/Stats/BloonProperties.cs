using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "BloonProperties", menuName = "Scriptable Objects/Bloon Properties", order = 0)]
    public class BloonProperties : ScriptableObject
    {
        [SerializeField] private Color _bloonColor;
        [SerializeField] private bool _canBePoppedByDarts;
        [SerializeField] private bool _canBePoppedByTacks;
        [SerializeField] private bool _canBePoppedByBombs;
        [SerializeField] private bool _canBeFrozen;
        [SerializeField] private bool _canBeDetected;
        [SerializeField] private int _numberOfHitsToPop;
        [SerializeField] private int _moneyWhenPopped;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private List<BloonProperties> _bloonsToSpawnWhenPopped;
        [SerializeField] private BloonTypes _bloonType;

        private List<BloonProperties> _previousBloonsToSpawnWhenPopped;
        
        //TODO add in collider size and/or shape

        public Color BloonColor
        {
            get => _bloonColor;
            private set => _bloonColor = value;
        }

        public bool CanBePoppedByDarts
        {
            get => _canBePoppedByDarts;
            private set => _canBePoppedByDarts = value;
        }

        public bool CanBePoppedByTacks
        {
            get => _canBePoppedByTacks;
            private set => _canBePoppedByTacks = value;
        }

        public bool CanBePoppedByBombs
        {
            get => _canBePoppedByBombs;
            private set => _canBePoppedByBombs = value;
        }

        public bool CanBeFrozen
        {
            get => _canBeFrozen;
            private set => _canBeFrozen = value;
        }

        public bool CanBeDetected
        {
            get => _canBeDetected;
            private set => _canBeDetected = value;
        }

        public int NumberOfHitsToPop
        {
            get => _numberOfHitsToPop;
            private set => _numberOfHitsToPop = Math.Max(1, value);
        }

        public int MoneyWhenPopped => _moneyWhenPopped;
        
        public float MoveSpeed
        {
            get => _moveSpeed;
            private set => _moveSpeed = value;
        }

        public List<BloonProperties> BloonsToSpawnWhenPopped
        {
            get => _bloonsToSpawnWhenPopped;
            set
            {
                _bloonsToSpawnWhenPopped = value;
                var parentList = new List<BloonTypes>();
                if (!ValidateChildBloons(parentList))
                {
                    _bloonsToSpawnWhenPopped.Clear();
                    if (_previousBloonsToSpawnWhenPopped != null && _previousBloonsToSpawnWhenPopped.Count > 0)
                    {
                        _bloonsToSpawnWhenPopped = new List<BloonProperties>(_previousBloonsToSpawnWhenPopped);
                    }

                    throw new ArgumentException(
                        "Error: Loop detected in Bloons to spawn list. Reverting changes.");
                }
                else
                {
                    _previousBloonsToSpawnWhenPopped = _bloonsToSpawnWhenPopped;
                }
            }
        }
        
        private void OnValidate()
        {
            BloonsToSpawnWhenPopped = _bloonsToSpawnWhenPopped;
            _previousBloonsToSpawnWhenPopped = new List<BloonProperties>(_bloonsToSpawnWhenPopped);
        }
        
        private bool ValidateChildBloons(List<BloonTypes> parentBloons)
        {
            if (_bloonsToSpawnWhenPopped == null || _bloonsToSpawnWhenPopped.Count <= 0) return true;
            if (parentBloons.Contains(_bloonType)) return false;
            parentBloons.Add(_bloonType);
            foreach (var childBloon in _bloonsToSpawnWhenPopped)
            {
                if(childBloon == null) continue;
                if (!childBloon.ValidateChildBloons(new List<BloonTypes>(parentBloons)))
                {
                    return false;
                }
            }
            return true;
        }
        
        //TODO: Validate existence of only one Scriptable Object per Bloon Type.
        public BloonTypes BloonType
        {
            get => _bloonType;
            private set => _bloonType = value;
        }
        
        public int RedBloonEquivalent
        {
            get
            {
                int rbe = _numberOfHitsToPop;
                if (_bloonsToSpawnWhenPopped == null || _bloonsToSpawnWhenPopped.Count <= 0) return rbe;

                foreach (var spawnedBloon in _bloonsToSpawnWhenPopped)
                {
                    rbe += spawnedBloon.RedBloonEquivalent;
                }
                return rbe;
            }
        }

        public int TotalBloonCount
        {
            get
            {
                int totalBloonCount = 1;
                if (_bloonsToSpawnWhenPopped == null || _bloonsToSpawnWhenPopped.Count <= 0) return totalBloonCount;

                foreach (var spawnedBloon in _bloonsToSpawnWhenPopped)
                {
                    totalBloonCount += spawnedBloon.TotalBloonCount;
                }

                return totalBloonCount;
            }
        }
    }
}