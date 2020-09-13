using UnityEngine;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.UI;

namespace TMG.BloonsTD.Controllers
{
    public class RoundController : MonoBehaviour
    {
        private GameStatistics _currentGameStatistics;

        public GameStatistics CurrentGameStatistics
        {
            get => _currentGameStatistics;
            set => _currentGameStatistics = value;
        }

        
    }
}