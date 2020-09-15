using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "Round-", menuName = "Round Spawn Statistics", order = 0)]
    public class RoundSpawnStatistics : ScriptableObject
    {
        [SerializeField] private List<SpawnGroup> _spawnGroups;

        public List<SpawnGroup> SpawnGroups => _spawnGroups;
    }
}