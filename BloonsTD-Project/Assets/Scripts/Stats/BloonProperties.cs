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
            set => _numberOfHitsToPop = value;
        }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public List<BloonProperties> BloonsToSpawnWhenPopped
        {
            get => _bloonsToSpawnWhenPopped;
            set => _bloonsToSpawnWhenPopped = value;
        }

        public int RedBloonEquivalent
        {
            get
            {
                int rbe = _numberOfHitsToPop;
                if (_bloonsToSpawnWhenPopped == null || _bloonsToSpawnWhenPopped.Count <= 0) return rbe;

                foreach (var spawnedBloons in _bloonsToSpawnWhenPopped)
                {
                    rbe += spawnedBloons.RedBloonEquivalent;
                }
                return rbe;
            }
            set => throw new System.NotImplementedException();
        }
    }
}