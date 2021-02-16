using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        public delegate void StatChangedDelegate(int newStatValue);

        public event StatChangedDelegate OnRoundChanged;
        public event StatChangedDelegate OnMoneyChanged;
        public event StatChangedDelegate OnLivesChanged;

        public delegate void GameEvent();

        public event GameEvent OnGameBegin;
        public event GameEvent OnGameOver;
        
        [SerializeField] private GameStatistics _startingGameStatistics;
        [SerializeField] private GameStatistics _currentGameStatistics;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private BloonSpawner _bloonSpawner;

        private bool _gameOver;
        public bool GameOver => _gameOver;
        public int Money => _currentGameStatistics.Money;
        private int BaseRewardAmount => _currentGameStatistics.Round1Reward + 1;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeStatistics();
            InitializeUI();
        }
        
        public void SetupNewGame()
        {
            InitializeStatistics();
            InitializeUI();
            OnGameBegin?.Invoke();
        }

        private void OnEnable()
        {
            BloonSpawner.Instance.OnBloonSpawned += SetupBloonEvents;
            _roundController.OnRoundComplete += RoundComplete;
        }

        private void OnDisable()
        {
            BloonSpawner.Instance.OnBloonSpawned -= SetupBloonEvents;
            _roundController.OnRoundComplete -= RoundComplete;
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
            OnRoundChanged?.Invoke(_currentGameStatistics.Rounds);
            OnMoneyChanged?.Invoke(_currentGameStatistics.Money);
            OnLivesChanged?.Invoke(_currentGameStatistics.Lives);
        }

        public void BeginNewRound()
        {
            IncrementRound();
            _roundController.StartRound(_currentGameStatistics.Rounds);
        }

        private void RoundComplete(int roundNumber)
        {
            EndOfRoundReward(roundNumber);
        }

        private void EndOfRoundReward(int roundNumber)
        {
            IncrementMoney(BaseRewardAmount - roundNumber);
        }

        private void IncrementRound()
        {
            _currentGameStatistics.Rounds++;
            OnRoundChanged?.Invoke(_currentGameStatistics.Rounds);
        }

        private void IncrementMoney(int amount)
        {
            _currentGameStatistics.Money += amount;
            OnMoneyChanged?.Invoke(_currentGameStatistics.Money);
        }

        public void DecrementMoney(int amount)
        {
            _currentGameStatistics.Money -= amount;
            OnMoneyChanged?.Invoke(_currentGameStatistics.Money);
        }

        private void DecrementLives(int livesToLose)
        {
            if(_gameOver){return;}
            
            _currentGameStatistics.Lives -= livesToLose;
            OnLivesChanged?.Invoke(_currentGameStatistics.Lives);
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
            Debug.Log("Game Over");
            _gameOver = true;
            OnGameOver?.Invoke();
        }
    }
}