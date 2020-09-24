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
            _roundController.BloonSpawner = _bloonSpawner;
            _roundController.OnBloonSpawned += SetupBloonEvents;
            _roundController.OnRoundComplete += _generalUIController.ShowStartRoundButton;
        }

        private void SetupBloonEvents(BloonController bloonController)
        {
            bloonController.OnBloonReachedEndOfPath += DecrementLives;
        }

        private void InitializeStatistics()
        {
            _currentGameStatistics.SetGameStatistics(_startingGameStatistics);
        }

        //TODO: Change UI updating to an event
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

        private void DecrementLives(int livesToLose)
        {
            _currentGameStatistics.Lives -= livesToLose;
            _generalUIController.UpdateLivesValue(_currentGameStatistics.Lives.ToString());
            if (_currentGameStatistics.Lives <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            Debug.Log("Game Over");
        }
    }
}