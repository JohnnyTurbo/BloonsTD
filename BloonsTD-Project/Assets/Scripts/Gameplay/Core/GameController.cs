using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public delegate void UpdateUITextDelegate(string newText);

        public event UpdateUITextDelegate OnRoundChanged;
        public event UpdateUITextDelegate OnMoneyChanged;
        public event UpdateUITextDelegate OnLivesChanged;
        
        [SerializeField] private GameStatistics _startingGameStatistics;
        [SerializeField] private GameStatistics _currentGameStatistics;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private BloonSpawner _bloonSpawner;

        private bool _gameOver;
        public bool GameOver => _gameOver;
        public int Money => _currentGameStatistics.Money;

        private void Start()
        {
            //TODO: Will be called when New Game Button is clicked
            SetupNewGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Time.timeScale = -1f;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Time.timeScale = 1f;
            }
        }

        private void SetupNewGame()
        {
            SetupEvents();
            InitializeStatistics();
            InitializeUI();
        }

        private void SetupEvents()
        {
            BloonSpawner.Instance.OnBloonSpawned += SetupBloonEvents;
        }

        private void SetupBloonEvents(BloonController bloonController)
        {
            bloonController.OnBloonReachedEndOfPath += BloonEndOfPath;
            bloonController.OnBloonPopped += BloonPopped;
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
            IncrementRound();
            _roundController.StartRound(_currentGameStatistics.Rounds);
        }

        private void IncrementRound()
        {
            _currentGameStatistics.Rounds++;
            OnRoundChanged?.Invoke(_currentGameStatistics.Rounds.ToString());
        }

        private void IncrementMoney(int amount)
        {
            _currentGameStatistics.Money += amount;
            OnMoneyChanged?.Invoke(_currentGameStatistics.Money.ToString());
        }

        public void DecrementMoney(int amount)
        {
            _currentGameStatistics.Money -= amount;
            OnMoneyChanged?.Invoke(_currentGameStatistics.Money.ToString());
        }

        private void DecrementLives(int livesToLose)
        {
            if(_gameOver){return;}
            
            _currentGameStatistics.Lives -= livesToLose;
            OnLivesChanged?.Invoke(_currentGameStatistics.Lives.ToString());
            if (_currentGameStatistics.Lives <= 0)
            {
                BeginGameOver();
            }
        }

        private void BloonEndOfPath(BloonProperties bloonProperties)
        {
            DecrementLives(bloonProperties.TotalBloonCount);
        }

        private void BloonPopped(BloonProperties bloonProperties)
        {
            IncrementMoney(bloonProperties.MoneyWhenPopped);
            _currentGameStatistics.NumBloonsPopped++;
        }

        private void BeginGameOver()
        {
            _gameOver = true;
            Debug.Log("Game Over");
        }
    }
}