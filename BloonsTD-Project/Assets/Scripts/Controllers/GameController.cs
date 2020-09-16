using System;
using UnityEngine;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.UI;

namespace TMG.BloonsTD.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameStatistics _startingGameStatistics;
        [SerializeField] private GameStatistics _currentGameStatistics;
        [SerializeField] private GeneralUIController _generalUIController;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private BloonSpawner _bloonSpawner;
        
        private void Start()
        {
            // Will be called when New Game Button is clicked
            SetupNewGame();
        }

        private void SetupNewGame()
        {
            InitializeControllers();
            InitializeStatistics();
            InitializeUI();
        }

        private void InitializeControllers()
        {
            //_roundController.PathController = _pathController;
            _roundController.BloonSpawner = _bloonSpawner;
        }

        private void InitializeStatistics()
        {
            _currentGameStatistics.SetGameStatistics(_startingGameStatistics);
        }

        private void InitializeUI()
        {
            _generalUIController.UpdateRoundValue(_currentGameStatistics.Round.ToString());
            _generalUIController.UpdatedMoneyValue(_currentGameStatistics.Money.ToString());
            _generalUIController.UpdateLivesValue(_currentGameStatistics.Lives.ToString());
        }
        
        public void BeginNewRound()
        {
            _currentGameStatistics.Round++;
            _generalUIController.UpdateRoundValue(_currentGameStatistics.Round.ToString());
            _roundController.StartRound(_currentGameStatistics.Round);
        }
    }
}