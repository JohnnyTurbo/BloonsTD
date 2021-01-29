using System;
using System.Collections;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(TowerPlacementController))]
    [RequireComponent(typeof(TowerSelectionController))]
    [RequireComponent(typeof(TowerUpgradeController))]
    public class TowerController : MonoBehaviour
    {
        [SerializeField] private TowerUpgradeController _towerUpgradeController;
        [SerializeField] private TowerBloonDetector _towerBloonDetector;
        
        private TowerState _currentTowerState;
        private TowerAttack _towerAttack;
        
        public TowerProperties TowerProperties { get; private set; }
        public TowerPlacementController PlacementController { get; private set; }
        public TowerSelectionController SelectionController { get; private set; }
        public TowerUpgradeController TowerUpgradeController => _towerUpgradeController;
        public TowerAttack TowerAttack => _towerAttack;

        public float TowerRange => TowerAttack.Range;

        public TowerState CurrentTowerState
        {
            get => _currentTowerState;
            set => _currentTowerState = value;
        }

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
            _towerAttack = TowerAttack.GetNewFromAttackType(TowerProperties.TowerAttackType);
            _towerBloonDetector.TowerController = this;
            _towerBloonDetector.SetRange(TowerProperties.Range);
            PlacementController.TowerProperties = TowerProperties;
            SelectionController.TowerController = this;
            TowerUpgradeController.InitializeUpgrades(this);
            _currentTowerState = TowerState.Placing;
            //TODO: Set renderer, collider, etc.
        }

        private void OnTowerPlaced(TowerController towerController)
        {
            TowerSpawnManager.Instance.OnTowerPlaced -= OnTowerPlaced;
            _currentTowerState = TowerState.Idle;
            _towerAttack.Initialize(this);
            _towerAttack.TryAttack();
        }

        public void OnBloonEnter()
        {
            if(TowerNotIdle) {return;}

            _towerAttack.TryAttack();
        }

        private void TryAttack()
        {
            _towerAttack.TryAttack();
        }

        public void StartCooldownTimer(WaitForSeconds cooldownTime)
        {
            StartCoroutine(AttackCooldownTimer(cooldownTime));
        }
        private IEnumerator AttackCooldownTimer(WaitForSeconds cooldownTime)
        {
            CurrentTowerState = TowerState.Cooldown;
            yield return cooldownTime;
            CurrentTowerState = TowerState.Idle;
            TryAttack();
        }
    }
}