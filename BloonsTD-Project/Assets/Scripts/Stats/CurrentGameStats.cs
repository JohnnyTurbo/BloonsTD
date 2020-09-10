using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    public class CurrentGameStats
    {
        private int _round;
        private int _money;
        private int _lives;
        
        //Add later, num towers built, total bloons popped, etc.

        // TODO: define max round elsewhere
        public int Round
        {
            get => _round;
            set => _round = Mathf.Clamp(value, 1, 50);
        }

        public int Money
        {
            get => _money;
            set => _money = value;
        }

        // TODO: define max lives elsewhere
        public int Lives
        {
            get => _lives;
            set => _lives = Mathf.Clamp(value, 0, 40);
        }

        public CurrentGameStats(int round, int money, int lives)
        {
            _round = round;
            _money = money;
            _lives = lives;
        }

        public CurrentGameStats()
        {
            
        }
    }
}