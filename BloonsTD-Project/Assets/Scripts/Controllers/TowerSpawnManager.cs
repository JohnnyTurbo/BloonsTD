using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public enum TowerPlacementState
    {
        NotPlacingTower,
        PlacingTower,
        CannotPlaceTower
    }
    public class TowerSpawnManager : MonoBehaviour
    {
        public static TowerSpawnManager Instance;
        
        
        [SerializeField] private GameController _gameController;
        [SerializeField] private GameObject _towerPrefab;
        
        private TowerPlacementState _towerPlacementState;
        private GameObject _currentTowerGO;
        private TowerPlacementController _currentTowerPlacementController;

        public TowerPlacementState TowerPlacementState => _towerPlacementState;

        private bool IsPlacingTower =>
            _towerPlacementState == TowerPlacementState.PlacingTower && _currentTowerPlacementController != null;
        private bool CanPlaceTower => IsPlacingTower && _currentTowerPlacementController.IsValidPlacementPosition;

        public delegate void TowerPlaced();

        public TowerPlaced OnTowerPlaced;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _towerPlacementState = TowerPlacementState.NotPlacingTower;
            _currentTowerPlacementController = null;
        }

        public void OnButtonSpawnTower(TowerProperties tower)
        {
            if (_gameController.Money < tower.Cost)
            {
                Debug.LogWarning($"Cannot afford tower, get richer. Need {tower.Cost - _gameController.Money} more $.");
                return;
            }
            
            switch (_towerPlacementState)
            {
                case TowerPlacementState.NotPlacingTower:
                    _towerPlacementState = TowerPlacementState.PlacingTower;
                    break;
                
                case TowerPlacementState.PlacingTower:
                    Debug.Log($"No longer placing tower: {_currentTowerPlacementController.TowerProperties.Name}");
                    break;
                
                case TowerPlacementState.CannotPlaceTower:
                    return;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log($"Spawning {tower.Name}");
            _currentTowerGO = Instantiate(_towerPrefab, Vector3.zero, Quaternion.identity);
            _currentTowerPlacementController = _currentTowerGO.GetComponent<TowerPlacementController>();
            _currentTowerPlacementController.TowerProperties = tower;
            //TODO: Call some setup method to set renderer, collider, etc.
        }

        //TODO: Valid Placement Checking
        private void Update()
        {
            if (IsPlacingTower)
            {
                _currentTowerPlacementController.transform.position = InputController.TowerPlacementPosition;
            }
            if (InputController.PlaceTowerFlag && CanPlaceTower)
            {
                OnTowerPlaced?.Invoke();
                Debug.Log($"Placing tower: {_currentTowerPlacementController.TowerProperties.Name}");
                _towerPlacementState = TowerPlacementState.NotPlacingTower;
                Debug.Log($"Decrementing money by {_currentTowerPlacementController.TowerProperties.Cost}");
                _gameController.DecrementMoney(_currentTowerPlacementController.TowerProperties.Cost);
            }
        }
    }
}