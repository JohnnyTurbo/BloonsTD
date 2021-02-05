using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [SerializeField] private TowerSpawnManager _towerSpawnManager;

        private TowerSelectionController _selectedTower;
        private bool TowerInPlacingState => _towerSpawnManager.TowerPlacementState == TowerPlacementState.PlacingTower;
        private bool _canSelectTowers;

        private void Start()
        {
            //TODO: put this in some sort of initializer event
            _canSelectTowers = true;
        }

        private void OnEnable()
        {
            GameController.Instance.OnGameOver += GameOver;
        }
        private void OnDisable()
        {
            GameController.Instance.OnGameOver -= GameOver;
        }

        private void Update()
        {
            //TODO: Cleanup!
            if (TowerInPlacingState && InputController.BeginCancelSelection)
            {
                DeselectTower();
                _towerSpawnManager.CancelPlacingTower();
                return;
            }
            else if(TowerInPlacingState)
            {
                return;
            }

            if (InputController.BeginSelectScreen)
            {
                AttemptTowerSelection();
            }

            if (InputController.BeginCancelSelection)
            {
                DeselectTower();
            }
        }

        private void AttemptTowerSelection()
        {
            if(!_canSelectTowers) {return;}
            var worldSelectionPosition = InputController.WorldSelectionPosition;
            var towerFiler = new ContactFilter2D {layerMask = 1 << PhysicsLayers.Towers, useLayerMask = true};
            var towerColliders = new List<Collider2D>();
            var numTowersSelected = Physics2D.OverlapPoint(worldSelectionPosition, towerFiler, towerColliders);
            if (numTowersSelected > 0)
            {
                var selectedTower = towerColliders[0].GetComponent<TowerSelectionController>();
                SelectTower(selectedTower);
            }
            else
            {
                DeselectTower();
            }
        }

        private void SelectTower(TowerSelectionController selectedTower)
        {
            if (_selectedTower != null)
            {
                DeselectTower();
            }

            selectedTower.SelectTower();
            _selectedTower = selectedTower;
        }

        public void DeselectTower()
        {
            if (_selectedTower == null) { return; }
            _selectedTower.DeselectTower();
        }

        private void GameOver()
        {
            DeselectTower();
            _canSelectTowers = false;
        }
    }
}