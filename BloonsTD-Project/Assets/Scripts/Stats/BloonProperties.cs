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

        public Color BloonColor => _bloonColor;
        public bool CanBePoppedByDarts => _canBePoppedByDarts;
        public bool CanBePoppedByTacks => _canBePoppedByTacks;
        public bool CanBePoppedByBombs => _canBePoppedByBombs;
        public bool CanBeFrozen => _canBeFrozen;
        public bool CanBeDetected => _canBeDetected;
        public int NumberOfHitsToPop => _numberOfHitsToPop;
        public float MoveSpeed => _moveSpeed;
        public List<BloonProperties> BloonsToSpawnWhenPopped => _bloonsToSpawnWhenPopped;
    }
}