using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "Round-", menuName = "Round Spawn Statistics", order = 0)]
    public class RoundSpawnStatistics : ScriptableObject
    {
        [SerializeField] private List<SpawnGroup> _spawnGroups;

        public List<SpawnGroup> SpawnGroups => _spawnGroups;

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

        public int NumberOfBloons
        {
            get
            {
                int numberOfBloons = 0;

                foreach (var spawnGroup in _spawnGroups)
                {
                    numberOfBloons += spawnGroup.NumberInGroup;
                }

                return numberOfBloons;
            }
        }

        //TODO: Implement total round RBE
    }
}