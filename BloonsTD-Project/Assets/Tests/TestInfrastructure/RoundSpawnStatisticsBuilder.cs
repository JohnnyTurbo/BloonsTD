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

        private RoundProperties Build()
        {
            var roundSpawnStatistics = ScriptableObject.CreateInstance<RoundProperties>();
            roundSpawnStatistics.SpawnGroups = _spawnGroups;
            return roundSpawnStatistics;
        }

        public static implicit operator RoundProperties(RoundSpawnStatisticsBuilder builder)
        {
            return builder.Build();
        }
    }
}