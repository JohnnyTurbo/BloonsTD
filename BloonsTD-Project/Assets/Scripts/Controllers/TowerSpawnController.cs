using System;
using TMG.BloonsTD.Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMG.BloonsTD.Controllers
{
    public class TowerSpawnController : MonoBehaviour
    {
        private enum TowerPlacementState
        {
            NotPlacingTower,
            PlacingTower,
            CannotPlaceTower
        }
        [SerializeField] private GameController _gameController;
        [SerializeField] private GameObject _towerPrefab;
        
        private TowerPlacementState _towerPlacementState;
        private TowerController _currentTowerController;

        private bool IsPlacingTower =>
            _towerPlacementState == TowerPlacementState.PlacingTower && _currentTowerController != null;
        private bool CanPlaceTower => IsPlacingTower && _currentTowerController.ValidatePlacementPosition();
        private void Start()
        {
            _towerPlacementState = TowerPlacementState.NotPlacingTower;
            _currentTowerController = null;
        }

        public void SpawnTower(TowerProperties tower)
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
                    Debug.Log($"No longer placing tower: {_currentTowerController.TowerProperties.Name}");
                    break;
                
                case TowerPlacementState.CannotPlaceTower:
                    return;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
            Debug.Log($"Spawning {tower.Name}");
            GameObject newTower = Instantiate(_towerPrefab, Vector3.zero, Quaternion.identity);
            _currentTowerController = newTower.GetComponent<TowerController>();
            _currentTowerController.TowerProperties = tower;
        }

        //TODO: Valid Placement Checking
        private void Update()
        {
            if (InputController.ReadPlaceTower() && CanPlaceTower)
            {
                Debug.Log($"Placing tower: {_currentTowerController.TowerProperties.Name}");
                _towerPlacementState = TowerPlacementState.NotPlacingTower;
                Debug.Log($"Decrementing money by {_currentTowerController.TowerProperties.Cost}");
                _gameController.DecrementMoney(_currentTowerController.TowerProperties.Cost);
            }
        }
    }
}