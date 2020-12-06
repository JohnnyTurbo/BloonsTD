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
        public TowerStatistics TowerStatistics { get; private set; }
        public TowerPlacementController PlacementController { get; private set; }
        public TowerSelectionController SelectionController { get; private set; }
        public TowerTargetType TowerTargetType { get; private set; }
        
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

            TowerStatistics = ScriptableObject.CreateInstance<TowerStatistics>();
            TowerStatistics.Set(towerProperties);

            _attackCooldownTime = new WaitForSeconds(TowerStatistics.AttackCooldownTime);
            PlacementController.TowerProperties = towerProperties;
            SelectionController.TowerStatistics = TowerStatistics;
            
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