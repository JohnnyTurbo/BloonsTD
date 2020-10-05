using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.TestInfrastructure
{
    public class GameStatisticsBuilder
    {
        private int _rounds;
        private int _money;
        private int _lives;
        private GameStatistics _maxGameStatistics;

        public GameStatisticsBuilder WithRounds(int rounds)
        {
            _rounds = rounds;
            return this;
        }

        public GameStatisticsBuilder WithMoney(int money)
        {
            _money = money;
            return this;
        }

        public GameStatisticsBuilder WithLives(int lives)
        {
            _lives = lives;
            return this;
        }

        public GameStatisticsBuilder WithMaxStatistics(GameStatistics maxGameStatistics)
        {
            _maxGameStatistics = maxGameStatistics;
            return this;
        }
        
        private GameStatistics Build()
        {
            var newGameStatistics = ScriptableObject.CreateInstance<GameStatistics>();
            newGameStatistics.MaxGameStatistics = _maxGameStatistics;
            newGameStatistics.Rounds = _rounds;
            newGameStatistics.Money = _money;
            newGameStatistics.Lives = _lives;
            return newGameStatistics;
        }

        public static implicit operator GameStatistics(GameStatisticsBuilder builder)
        {
            return builder.Build();
        }
    }
}