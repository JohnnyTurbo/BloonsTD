using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "BloonProperties", menuName = "New Bloon Properties", order = 0)]
    public class BloonProperties : ScriptableObject
    {
        [SerializeField] private Color _bloonColor;
        [SerializeField] private bool _canBePoppedByDarts;
        [SerializeField] private bool _canBePoppedByTacks;
        [SerializeField] private bool _canBePoppedByBombs;
        [SerializeField] private bool _canBeFrozen;
        [SerializeField] private bool _canBeDetected;
        [SerializeField] private int _numberOfHitsToPop;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private List<BloonProperties> _bloonsToSpawnWhenPopped;
        [SerializeField] private BloonTypes _bloonType;

        //TODO add in collider size and/or shape

        public Color BloonColor
        {
            get => _bloonColor;
            set => _bloonColor = value;
        }

        public bool CanBePoppedByDarts
        {
            get => _canBePoppedByDarts;
            set => _canBePoppedByDarts = value;
        }

        public bool CanBePoppedByTacks
        {
            get => _canBePoppedByTacks;
            set => _canBePoppedByTacks = value;
        }

        public bool CanBePoppedByBombs
        {
            get => _canBePoppedByBombs;
            set => _canBePoppedByBombs = value;
        }

        public bool CanBeFrozen
        {
            get => _canBeFrozen;
            set => _canBeFrozen = value;
        }

        public bool CanBeDetected
        {
            get => _canBeDetected;
            set => _canBeDetected = value;
        }

        public int NumberOfHitsToPop
        {
            get => _numberOfHitsToPop;
            set => _numberOfHitsToPop = Math.Max(1, value);
        }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public List<BloonProperties> BloonsToSpawnWhenPopped
        {
            get => _bloonsToSpawnWhenPopped;
            set
            {
                var initialBloonsToSpawn = _bloonsToSpawnWhenPopped;
                _bloonsToSpawnWhenPopped = value;
                var parentList = new List<BloonProperties>();
                if (!ValidateChildBloons(parentList))
                {
                    _bloonsToSpawnWhenPopped = initialBloonsToSpawn;
                    throw new ArgumentException(
                        "Error: Loop detected in Bloons to spawn list. Reverting changes.");
                }
                //_bloonsToSpawnWhenPopped = value;
            }
        }

        private void OnValidate()
        {
            BloonsToSpawnWhenPopped = _bloonsToSpawnWhenPopped;
        }

        // TODO: Properly revert changes upon failed attempt to set bloons to spawn.
        private bool ValidateChildBloons(List<BloonProperties> parentBloons)
        {
            if (_bloonsToSpawnWhenPopped == null || _bloonsToSpawnWhenPopped.Count <= 0) return true;
            if (parentBloons.Contains(this)) return false;
            parentBloons.Add(this);
            foreach (var childBloon in _bloonsToSpawnWhenPopped)
            {
                if(childBloon == null) continue;
                if (!childBloon.ValidateChildBloons(parentBloons))
                {
                    return false;
                }
            }
            return true;
        }
        
        public BloonTypes BloonType
        {
            get => _bloonType;
            set => _bloonType = value;
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