using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "GameStatistics", menuName = "Scriptable Objects/Game Statistics", order = 0)]
    public class GameStatistics : ScriptableObject
    {
        [SerializeField] private int _rounds;
        [SerializeField] private int _money;
        [SerializeField] private int _lives;
        [SerializeField] private GameStatistics _maxGameStatistics;

        private int _numBloonsPopped;
        private bool _hasMaxStatistics;

        public bool HasMaxStatistics
        {
            get => _hasMaxStatistics;
            set => _hasMaxStatistics = value;
        }
        private void Awake()
        {
            if (_maxGameStatistics != null)
            {
                _hasMaxStatistics = true;
            }
        }

        public int Rounds
        {
            get => _rounds;
            set =>
                _rounds = _hasMaxStatistics && !_maxGameStatistics.Rounds.Equals(0)
                    ? Mathf.Clamp(value, 0, _maxGameStatistics.Rounds)
                    : Mathf.Max(0, value);
        }

        public int Money
        {
            get => _money;
            set =>
                _money = _hasMaxStatistics && !_maxGameStatistics.Money.Equals(0)
                    ? Mathf.Clamp(value, 0, _maxGameStatistics.Money)
                    : Mathf.Max(0, value);
        }

        public int Lives
        {
            get => _lives;
            set =>
                _lives = _hasMaxStatistics && !_maxGameStatistics.Lives.Equals(0)
                    ? Mathf.Clamp(value, 0, _maxGameStatistics.Lives)
                    : Mathf.Max(0, value);
        }

        public int NumBloonsPopped
        {
            get => _numBloonsPopped;
            set => _numBloonsPopped = Mathf.Max(0, value);
        }
        
        public GameStatistics MaxGameStatistics
        {
            get => _maxGameStatistics;
            set => _maxGameStatistics = value;
        }

        public void SetGameStatistics(GameStatistics newGameStatistics)
        {
            Rounds = newGameStatistics.Rounds;
            Money = newGameStatistics.Money;
            Lives = newGameStatistics.Lives;
            NumBloonsPopped = newGameStatistics.NumBloonsPopped;
        }
    }
}