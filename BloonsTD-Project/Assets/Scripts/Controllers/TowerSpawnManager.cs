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
        private TowerController _curTowerController;
        public TowerPlacementState TowerPlacementState => _towerPlacementState;

        private bool IsPlacingTower =>
            _towerPlacementState == TowerPlacementState.PlacingTower && _curTowerController != null;

        private bool CanPlaceTower => IsPlacingTower && _curTowerController.Placement.IsValidPlacementPosition;

        public delegate void TowerPlaced(TowerController towerController);

        public TowerPlaced OnTowerPlaced;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _towerPlacementState = TowerPlacementState.NotPlacingTower;
            _curTowerController = null;
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
                    Debug.Log($"No longer placing tower: {_curTowerController.TowerProperties.Name}");
                    break;
                
                case TowerPlacementState.CannotPlaceTower:
                    return;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SpawnTower(tower);
        }

        private void SpawnTower(TowerProperties tower)
        {
            //Debug.Log($"Spawning {tower.Name}");
            _currentTowerGO = Instantiate(_towerPrefab, Vector3.zero, Quaternion.identity);
            _curTowerController = _currentTowerGO.GetComponent<TowerController>();
            _curTowerController.InitializeTower(tower);
        }

        //TODO: Valid Placement Checking
        private void Update()
        {
            if (IsPlacingTower)
            {
                _curTowerController.transform.position = InputController.TowerPlacementPosition;
            }
            if (InputController.PlaceTowerFlag && CanPlaceTower)
            {
                OnTowerPlaced?.Invoke(_curTowerController);
                //Debug.Log($"Placing tower: {_curTowerController.TowerProperties.Name}");
                _towerPlacementState = TowerPlacementState.NotPlacingTower;
                //Debug.Log($"Decrementing money by {_curTowerController.TowerProperties.Cost}");
                _gameController.DecrementMoney(_curTowerController.TowerProperties.Cost);
            }
        }
    }
}