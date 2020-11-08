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
        
        public int Rounds
        {
            get => _rounds;
            set
            {
                if (_maxGameStatistics != null)
                {
                    _rounds = _maxGameStatistics.Rounds.Equals(0) ? value : Mathf.Clamp(value,0, _maxGameStatistics.Rounds);
                }
                else
                {
                    _rounds = Mathf.Max(0,value);
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
                    _money = _maxGameStatistics.Money.Equals(0) ? value : Mathf.Clamp(value,0, _maxGameStatistics.Money);
                }
                else
                {
                    _money = Mathf.Max(0,value);
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
                    _lives = _maxGameStatistics.Lives.Equals(0) ? value : Mathf.Clamp(value,0, _maxGameStatistics.Lives);
                }
                else
                {
                    _lives = Mathf.Max(0, value);
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
            Rounds = newGameStatistics.Rounds;
            Money = newGameStatistics.Money;
            Lives = newGameStatistics.Lives;
        }
    }
}