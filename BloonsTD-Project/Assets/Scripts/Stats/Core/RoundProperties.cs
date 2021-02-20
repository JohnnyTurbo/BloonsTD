using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "Round-", menuName = "Scriptable Objects/Round Spawn Statistics", order = 0)]
    public class RoundProperties : ScriptableObject
    {
        [SerializeField] private List<SpawnGroup> _spawnGroups;
        [SerializeField] [TextArea] private string _preRoundComment;
        private int _roundNumber;
        public List<SpawnGroup> SpawnGroups
        {
            get => _spawnGroups;
            set => _spawnGroups = value;
        }

        public string PreRoundComment => _preRoundComment;

        public int RoundNumber
        {
            get => _roundNumber;
            set => _roundNumber = Mathf.Max(1, value);
        }

        public float RoundTime
        {
            get
            {
                float maxGroupTime = 0f;

                foreach (var spawnGroup in _spawnGroups)
                {
                    float groupTime = spawnGroup.InitialSpawnDelay +
                                      spawnGroup.NumberInGroup * spawnGroup.TimeBetweenBloons;
                    maxGroupTime = Mathf.Max(maxGroupTime, groupTime);
                }
                
                return maxGroupTime;
            }
        }

        public int RedBloonEquivalent
        {
            get
            {
                int redBloonEquivalent = 0;

                foreach (var spawnGroup in _spawnGroups)
                {
                    redBloonEquivalent += spawnGroup.RedBloonEquivalent;
                }

                return redBloonEquivalent;
            }
        }
        
        public int TotalBloonCount
        {
            get
            {
                int totalBloonCount = 0;

                foreach (var spawnGroup in _spawnGroups)
                {
                    totalBloonCount += spawnGroup.TotalBloonCount;
                }

                return totalBloonCount;
            }
        }
    }
}