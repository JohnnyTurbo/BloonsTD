using System;
using UnityEngine;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.UI;

namespace TMG.BloonsTD.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private StartingStats _startingStats;
        [SerializeField] private GeneralUIController _generalUIController;

        private CurrentGameStats _currentGameStats;
        private void Start()
        {
            _currentGameStats = new CurrentGameStats
            {
                Round = _startingStats.StartingRound,
                Money = _startingStats.StartingMoney,
                Lives = _startingStats.StartingLives
            };

            InitializeUI();
        }

        private void InitializeUI()
        {
            _generalUIController.UpdateRoundValue(_currentGameStats.Round.ToString());
            _generalUIController.UpdatedMoneyValue(_currentGameStats.Money.ToString());
            _generalUIController.UpdateLivesValue(_currentGameStats.Lives.ToString());
        }
    }
}