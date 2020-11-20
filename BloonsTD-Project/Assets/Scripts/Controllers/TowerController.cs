using System;
using TMG.BloonsTD.Controllers.TowerAttackControllers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public enum TowerState
    {
        Placing,
        Idle,
        Cooldown
    }

    public enum TowerTargetType
    {
        First,
        Last,
        Strongest,
        Weakest
    }
    
    public class TowerController : MonoBehaviour
    {
        private TowerProperties _towerProperties;
        private TowerStatistics _towerStatistics;
        private TowerPlacementController _placement;
        private TowerSelectionController _selectionController;
        private TowerState _towerState;
        private TowerAttackController _towerAttackController;
        private TowerTargetType _towerTargetType;
        public TowerProperties TowerProperties => _towerProperties;
        public TowerStatistics TowerStatistics => _towerStatistics;
        public TowerPlacementController Placement => _placement;
        public TowerSelectionController SelectionController => _selectionController;
        public TowerState TowerState => _towerState;
        
        private void Awake()
        {
            _placement = GetComponent<TowerPlacementController>();
            if (_placement == null)
            {
                _placement = gameObject.AddComponent<TowerPlacementController>();
            }

            _selectionController = GetComponent<TowerSelectionController>();
            _towerAttackController = GetComponent<TowerAttackController>();
        }

        private void OnEnable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced += OnTowerPlaced;
        }

        private void OnDisable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced -= OnTowerPlaced;
        }

        private void OnTowerPlaced(TowerController towerController)
        {
            _towerState = TowerState.Idle;
        }

        public void OnBloonEnter()
        {
            if(_towerState != TowerState.Idle) {return;}
            _towerAttackController.TryAttack();
        }

        public void OnChangeTargetType(TowerTargetType newTargetType)
        {
            _towerTargetType = newTargetType;
        }
        
        public void InitializeTower(TowerProperties towerProperties)
        {
            _towerProperties = towerProperties;
            _towerStatistics = ScriptableObject.CreateInstance<TowerStatistics>();
            _towerStatistics.Set(towerProperties);
            _placement.TowerProperties = towerProperties;
            _selectionController.TowerStatistics = _towerStatistics;
            _towerState = TowerState.Placing;
            //TODO: Set renderer, collider, etc.
        }
    }
}