using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.TestInfrastructure
{
    public class SpawnGroupBuilder
    {
        private float _initialSpawnDelay;
        private BloonTypes _bloonType;
        private int _numberInGroup;
        private float _timeBetweenBloons;

        public SpawnGroupBuilder WithSpawnDelay(float initialSpawnDelay)
        {
            _initialSpawnDelay = initialSpawnDelay;
            return this;
        }

        public SpawnGroupBuilder OfBloonType(BloonTypes bloonType)
        {
            _bloonType = bloonType;
            return this;
        }

        public SpawnGroupBuilder WithNumberOfBloons(int numberInGroup)
        {
            _numberInGroup = numberInGroup;
            return this;
        }

        public SpawnGroupBuilder WithTimeBetweenBloons(float timeBetweenBloons)
        {
            _timeBetweenBloons = timeBetweenBloons;
            return this;
        }

        private SpawnGroup Build()
        {
            var spawnGroup = new SpawnGroup
            {
                InitialSpawnDelay = _initialSpawnDelay, 
                BloonType = _bloonType,
                NumberInGroup = _numberInGroup,
                TimeBetweenBloons = _timeBetweenBloons
            };

            return spawnGroup;
        }

        public static implicit operator SpawnGroup(SpawnGroupBuilder builder)
        {
            return builder.Build();
        }
    }
}