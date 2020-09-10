using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "StartingStats", menuName = "Starting Stats", order = 0)]
    public class StartingStats : ScriptableObject
    {
        [SerializeField] private int _startingRound;
        [SerializeField] private int _startingMoney;
        [SerializeField] private int _startingLives;

        public int StartingRound => _startingRound;
        public int StartingMoney => _startingMoney;
        public int StartingLives => _startingLives;
    }
}