using System;
using TMG.BloonsTD.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerSpawnManager : MonoBehaviour
    {
        public static TowerSpawnManager Instance;
        public delegate void TowerPlaced(TowerController towerController);
        public event TowerPlaced OnTowerPlaced;
        public delegate void DialogueMessageDelegate(string message);
        public event DialogueMessageDelegate OnDisplayMessage;
        
        [SerializeField] private GameController _gameController;
        [SerializeField] private GameObject _baseTowerPrefab;

        private TowerController _curTowerController;
        private GameObject _currentTowerGO;
        
        public TowerPlacementState TowerPlacementState { get; private set; }
        private bool IsPlacingTower =>
            TowerPlacementState == TowerPlacementState.PlacingTower && _curTowerController != null;
        private bool CanPlaceTower => IsPlacingTower && _curTowerController.PlacementController.IsValidPlacementPosition;
        private bool TryPlaceTower => InputController.BeginPlaceTower && CanPlaceTower;
        
        private void Awake()
        {
            InitializeControllerValues();
        }

        private void OnEnable()
        {
            _gameController.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            _gameController.OnGameOver -= GameOver;
        }

        private void Update()
        {
            if (IsPlacingTower)
            {
                _currentTowerGO.transform.position = InputController.TowerPlacementPosition;
            }

            if (TryPlaceTower)
            {
                OnTowerPlaced?.Invoke(_curTowerController);
                TowerPlacementState = TowerPlacementState.NotPlacingTower;
                _gameController.DecrementMoney(_curTowerController.TowerProperties.Cost);
                _curTowerController = null;
                _currentTowerGO = null;
            }
        }

        private void InitializeControllerValues()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            TowerPlacementState = TowerPlacementState.NotPlacingTower;
            _curTowerController = null;
        }

        public void OnButtonSpawnTower(TowerProperties tower)
        {
            if (CannotAffordTower(tower))
            {
                OnDisplayMessage?.Invoke("not enough money.");
                return;
            }
            
            switch (TowerPlacementState)
            {
                case TowerPlacementState.NotPlacingTower:
                    TowerPlacementState = TowerPlacementState.PlacingTower;
                    SpawnTower(tower);
                    break;
                
                case TowerPlacementState.PlacingTower:
                    Destroy(_currentTowerGO);
                    SpawnTower(tower);
                    break;
                
                case TowerPlacementState.CannotPlaceTower:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CannotAffordTower(TowerProperties tower)
        {
            return _gameController.Money < tower.Cost;
        }

        private void SpawnTower(TowerProperties tower)
        {
            _currentTowerGO = Instantiate(_baseTowerPrefab, Vector3.zero, Quaternion.identity);
            _curTowerController = _currentTowerGO.GetComponent<TowerController>();
            _curTowerController.InitializeTower(tower);
        }

        private void GameOver()
        {
            CancelPlacingTower();
            TowerPlacementState = TowerPlacementState.CannotPlaceTower;
        }
        
        public void CancelPlacingTower()
        {
            Destroy(_currentTowerGO);
            _currentTowerGO = null;
            _curTowerController = null;
            TowerPlacementState = TowerPlacementState.NotPlacingTower;
        }
    }
}