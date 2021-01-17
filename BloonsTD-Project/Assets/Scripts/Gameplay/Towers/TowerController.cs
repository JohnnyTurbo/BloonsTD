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
        private TowerState _currentTowerState;
        private TowerAttack _towerAttack;
        private WaitForSeconds _attackCooldownTime;
        public TowerProperties TowerProperties { get; private set; }
        public TowerPlacementController PlacementController { get; private set; }
        public TowerSelectionController SelectionController { get; private set; }
        public TowerUpgradeController UpgradeController { get; private set; }
        public TowerTargetType TowerTargetType { get; private set; }
        public WaitForSeconds AttackCooldownTime => _attackCooldownTime;
        public float AttackRange { get; private set; }

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
            UpgradeController = GetComponent<TowerUpgradeController>();
            
            _towerAttack = TowerAttack.GetNewAttackController(TowerProperties.TowerAttackType);
            _attackCooldownTime = new WaitForSeconds(TowerProperties.AttackCooldownTime);
            AttackRange = TowerProperties.Range;
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
            _towerAttack.Initialize(this);
            TryAttack();
        }

        public void OnBloonEnter()
        {
            if(TowerNotIdle) {return;}

            TryAttack();
        }

        private void TryAttack()
        {
            var towerAttackSuccess = _towerAttack.TryAttack();
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