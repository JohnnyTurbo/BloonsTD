using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.TestInfrastructure
{
    public class RoundSpawnStatisticsBuilder
    {
        private List<SpawnGroup> _spawnGroups;

        public RoundSpawnStatisticsBuilder WithSpawnGroups(List<SpawnGroup> spawnGroups)
        {
            _spawnGroups = spawnGroups;
            return this;
        }

        private RoundSpawnStatistics Build()
        {
            var roundSpawnStatistics = ScriptableObject.CreateInstance<RoundSpawnStatistics>();
            roundSpawnStatistics.SpawnGroups = _spawnGroups;
            return roundSpawnStatistics;
        }

        public static implicit operator RoundSpawnStatistics(RoundSpawnStatisticsBuilder builder)
        {
            return builder.Build();
        }
    }
}