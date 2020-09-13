using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "GameStatistics", menuName = "Game Statistics", order = 0)]
    public class GameStatistics : ScriptableObject
    {
        [SerializeField] private int _round;
        [SerializeField] private int _money;
        [SerializeField] private int _lives;
        [SerializeField] private GameStatistics _maxGameStatistics;
        
        public int Round
        {
            get => _round;
            set
            {
                if (_maxGameStatistics != null)
                {
                    _round = _maxGameStatistics.Round.Equals(0) ? value : Mathf.Min(value, _maxGameStatistics.Round);
                }
                else
                {
                    _round = value;
                }
            }
        }

        public int Money
        {
            get => _money;
            set
            {
                if (_maxGameStatistics != null)
                {
                    _money = _maxGameStatistics.Money.Equals(0) ? value : Mathf.Min(value, _maxGameStatistics.Money);
                }
                else
                {
                    _money = value;
                }
            }
        }

        public int Lives
        {
            get => _lives;
            set
            {
                if (_maxGameStatistics != null)
                {
                    _lives = _maxGameStatistics.Lives.Equals(0) ? value : Mathf.Min(value, _maxGameStatistics.Lives);
                }
                else
                {
                    _lives = value;
                }
            } 
        }

        public GameStatistics MaxGameStatistics
        {
            get => _maxGameStatistics;
            set => _maxGameStatistics = value;
        }

        public void SetGameStatistics(GameStatistics newGameStatistics)
        {
            Round = newGameStatistics.Round;
            Money = newGameStatistics.Money;
            Lives = newGameStatistics.Lives;
        }
    }
}