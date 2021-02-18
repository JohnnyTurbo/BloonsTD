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
        private Coroutine _attackCooldown;
        private int _totalTowerCost;
        public TowerProperties TowerProperties { get; private set; }
        public TowerPlacementController PlacementController { get; private set; }
        public TowerSelectionController SelectionController { get; private set; }
        public TowerUpgradeController TowerUpgradeController => _towerUpgradeController;
        public TowerAttack TowerAttack => _towerAttack;

        public float TowerRange => TowerAttack.Range;
        private float SellTowerMultiplier => GameController.Instance.SellTowerMultiplier;
        public int SellTowerCost => Mathf.FloorToInt(_totalTowerCost * SellTowerMultiplier);
        public TowerState CurrentTowerState
        {
            get => _currentTowerState;
            set => _currentTowerState = value;
        }

        private bool TowerNotIdle => _currentTowerState != TowerState.Idle;
        
        private void OnEnable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced += OnTowerPlaced;
            GameController.Instance.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced -= OnTowerPlaced;
            GameController.Instance.OnGameOver -= OnGameOver;
        }

        public void InitializeTower(TowerProperties towerProperties)
        {
            TowerProperties = towerProperties;
            PlacementController = GetComponent<TowerPlacementController>();
            SelectionController = GetComponent<TowerSelectionController>();
            _towerAttack = TowerAttack.GetNewFromAttackType(TowerProperties.TowerAttackType);
            _towerBloonDetector.TowerController = this;
            _towerBloonDetector.SetRange(TowerProperties.Range);
            _totalTowerCost = 0;
            PlacementController.TowerProperties = TowerProperties;
            SelectionController.TowerController = this;
            TowerUpgradeController.InitializeUpgrades(this);
            _currentTowerState = TowerState.Placing;
            Instantiate(TowerProperties.RendererPrefab, Vector3.zero, Quaternion.identity, transform);
            //TODO: Set collider, etc.
        }

        public void IncreaseTotalTowerCost(int costAmount)
        {
            _totalTowerCost += costAmount;
        }
        
        public void SellTower()
        {
            SelectionController.DeselectTower();
            GameController.Instance.IncrementMoney(SellTowerCost);
            StopAllCoroutines();
            Destroy(gameObject);
        }
        
        private void OnTowerPlaced(TowerController towerController)
        {
            _totalTowerCost += TowerProperties.Cost;
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
            _attackCooldown = StartCoroutine(AttackCooldownTimer(cooldownTime));
        }
        private IEnumerator AttackCooldownTimer(WaitForSeconds cooldownTime)
        {
            CurrentTowerState = TowerState.Cooldown;
            yield return cooldownTime;
            CurrentTowerState = TowerState.Idle;
            _attackCooldown = null;
            TryAttack();
        }

        private void OnGameOver()
        {
            if (_attackCooldown != null)
            {
                StopCoroutine(_attackCooldown);
            }
            CurrentTowerState = TowerState.GameOver;
        }
    }
}