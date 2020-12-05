using System;
using UnityEditor;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [System.Serializable]
    public class SpawnGroup
    {
        [Tooltip("Time delay between the first Bloon spawn of the previous group and this group.")]
        [SerializeField] private float _initialSpawnDelay;
        [SerializeField] private BloonTypes _bloonType;
        [SerializeField] private int _numberInGroup;
        [SerializeField] private float _timeBetweenBloons;

        public float InitialSpawnDelay
        {
            get => _initialSpawnDelay;
            set => _initialSpawnDelay = value;
        }

        public BloonTypes BloonType
        {
            get => _bloonType;
            set => _bloonType = value;
        }
        
        public int NumberInGroup
        {
            get => _numberInGroup;
            set => _numberInGroup = value;
        }

        public float TimeBetweenBloons
        {
            get => _timeBetweenBloons;
            set => _timeBetweenBloons = value;
        }

        public int RedBloonEquivalent
        {
            get
            {
                var bloonProperties = BloonPropertiesProcessor.GetBloonPropertiesFromBloonType(_bloonType);
                var redBloonEquivalent = bloonProperties.RedBloonEquivalent * _numberInGroup;
                return redBloonEquivalent;
            }
        }

        public int TotalBloonCount
        {
            get
            {
                var bloonProperties = BloonPropertiesProcessor.GetBloonPropertiesFromBloonType(_bloonType);
                var totalBloonCount = bloonProperties.TotalBloonCount * _numberInGroup;
                return totalBloonCount;
            }
        }
    }
}