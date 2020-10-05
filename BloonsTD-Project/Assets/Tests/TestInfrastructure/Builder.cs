namespace TMG.BloonsTD.TestInfrastructure
{
        public static class A
        {
            public static BloonPropertiesBuilder BloonProperties => new BloonPropertiesBuilder();
            public static GameStatisticsBuilder GameStatistics => new GameStatisticsBuilder();
            public static RoundSpawnStatisticsBuilder RoundSpawnStatistics => new RoundSpawnStatisticsBuilder();
            public static SpawnGroupBuilder SpawnGroup => new SpawnGroupBuilder();
        }
}