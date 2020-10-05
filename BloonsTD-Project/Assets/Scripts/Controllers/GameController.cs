using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameStatistics _startingGameStatistics;
        [SerializeField] private GameStatistics _currentGameStatistics;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private BloonSpawner _bloonSpawner;

        public delegate void UpdateUITextDelegate(string newText);

        public event UpdateUITextDelegate OnRoundChanged;
        public event UpdateUITextDelegate OnMoneyChanged;
        public event UpdateUITextDelegate OnLivesChanged;
        
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
        }

        private void SetupBloonEvents(BloonController bloonController)
        {
            bloonController.OnBloonReachedEndOfPath += DecrementLives;
        }

        private void InitializeStatistics()
        {
            _currentGameStatistics.SetGameStatistics(_startingGameStatistics);
        }
        
        private void InitializeUI()
        {
            OnRoundChanged?.Invoke(_currentGameStatistics.Rounds.ToString());
            OnMoneyChanged?.Invoke(_currentGameStatistics.Money.ToString());
            OnLivesChanged?.Invoke(_currentGameStatistics.Lives.ToString());
        }
        
        public void BeginNewRound()
        {
            _currentGameStatistics.Rounds++;
            OnRoundChanged?.Invoke(_currentGameStatistics.Rounds.ToString());
            _roundController.StartRound(_currentGameStatistics.Rounds);
        }

        private void DecrementLives(int livesToLose)
        {
            _currentGameStatistics.Lives -= livesToLose;
            OnLivesChanged?.Invoke(_currentGameStatistics.Lives.ToString());
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