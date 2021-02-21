using System;
using System.Net.NetworkInformation;
using TMG.BloonsTD.Stats;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private GameStatistics _curGameStatistics;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private BloonSpawner _bloonSpawner;
        
        private bool _gameOver;
        public bool GameOver => _gameOver;
        private bool _victory;
        public bool Victory => _victory;
        public int Money => _curGameStatistics.Money;
        private int BaseRewardAmount => _curGameStatistics.Round1Reward + 1;
        public float SellTowerMultiplier => _curGameStatistics.SellTowerMultiplier;
        private void Awake()
        {
            Instance = this;
            InputController.ResetMainCamera();
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
            _curGameStatistics.SetGameStatistics(_startingGameStatistics);
            _roundController.Initialize(_curGameStatistics);
        }

        private void InitializeUI()
        {
            OnRoundChanged?.Invoke(_curGameStatistics.Rounds);
            OnMoneyChanged?.Invoke(_curGameStatistics.Money);
            OnLivesChanged?.Invoke(_curGameStatistics.Lives);
        }

        public void BeginNewRound()
        {
            UpdateRoundText();
            _roundController.StartNextRound();
        }

        private void RoundComplete(RoundProperties completedRound)
        {
            EndOfRoundReward(completedRound.RoundNumber);
        }

        private void EndOfRoundReward(int roundNumber)
        {
            IncrementMoney(BaseRewardAmount - roundNumber);
        }

        private void UpdateRoundText()
        {
            OnRoundChanged?.Invoke(_curGameStatistics.Rounds);
        }

        public void IncrementMoney(int amount)
        {
            _curGameStatistics.Money += amount;
            OnMoneyChanged?.Invoke(_curGameStatistics.Money);
        }

        public void DecrementMoney(int amount)
        {
            _curGameStatistics.Money -= amount;
            OnMoneyChanged?.Invoke(_curGameStatistics.Money);
        }

        private void DecrementLives(int livesToLose)
        {
            if(_gameOver){return;}
            
            _curGameStatistics.Lives -= livesToLose;
            OnLivesChanged?.Invoke(_curGameStatistics.Lives);
            if (_curGameStatistics.Lives <= 0)
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
            _curGameStatistics.NumBloonsPopped++;
        }

        private void BeginGameOver()
        {
            _gameOver = true;
            OnGameOver?.Invoke();
        }

        public void BeginVictory()
        {
            _victory = true;
            BeginGameOver();
        }
    }
}