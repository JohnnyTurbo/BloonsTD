using System;
using System.Collections;
using TMG.BloonsTD.Gameplay.TowerAttackControllers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(TowerPlacementController))]
    [RequireComponent(typeof(TowerSelectionController))]
    [RequireComponent(typeof(TowerAttackController))]
    public class TowerController : MonoBehaviour
    {
        private TowerState _currentTowerState;
        private TowerAttackController _towerAttackController;
        private WaitForSeconds _attackCooldownTime;
        public TowerProperties TowerProperties { get; private set; }
        public TowerPlacementController PlacementController { get; private set; }
        public TowerSelectionController SelectionController { get; private set; }
        public TowerUpgradeController UpgradeController { get; private set; }
        public TowerTargetType TowerTargetType { get; private set; }

        

        public WaitForSeconds AttackCooldownTime => _attackCooldownTime;
        public float AttackRange { get; private set; }
        public GameObject ProjectilePrefab { get; private set; }

        private bool TowerNotIdle => _currentTowerState != TowerState.Idle;
        
        private void OnEnable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced += OnTowerPlaced;
        }

        private void OnDisable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced -= OnTowerPlaced;
        }

        public void InitializeTower(TowerProperties towerProperties)
        {
            TowerProperties = towerProperties;
            
            PlacementController = GetComponent<TowerPlacementController>();
            SelectionController = GetComponent<TowerSelectionController>();
            _towerAttackController = GetComponent<TowerAttackController>();
            UpgradeController = GetComponent<TowerUpgradeController>();
            _attackCooldownTime = new WaitForSeconds(TowerProperties.AttackCooldownTime);
            AttackRange = TowerProperties.Range;
            ProjectilePrefab = TowerProperties.ProjectilePrefab;
            PlacementController.TowerProperties = TowerProperties;
            SelectionController.TowerController = this;
            UpgradeController.InitializeUpgrades(this);
            
            
            TowerTargetType = TowerTargetType.First;
            _currentTowerState = TowerState.Placing;
            //TODO: Set renderer, collider, etc.
        }

        private void OnTowerPlaced(TowerController towerController)
        {
            _currentTowerState = TowerState.Idle;
            TryAttack();
        }

        public void OnBloonEnter()
        {
            if(TowerNotIdle) {return;}

            TryAttack();
        }

        private void TryAttack()
        {
            var towerAttackSuccess = _towerAttackController.TryAttack();
            if (!towerAttackSuccess) return;
            _currentTowerState = TowerState.Cooldown;
            StartCoroutine(AttackCooldownTimer());
        }

        private IEnumerator AttackCooldownTimer()
        {
            yield return _attackCooldownTime;
            _currentTowerState = TowerState.Idle;
            TryAttack();
        }
    }
}