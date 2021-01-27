using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [SerializeField] private TowerSpawnManager _towerSpawnManager;

        private TowerSelectionController _selectedTower;
        private bool TowerInPlacingState => _towerSpawnManager.TowerPlacementState == TowerPlacementState.PlacingTower;

        private void Update()
        {
            if (TowerInPlacingState && InputController.BeginCancelSelection)
            {
                Debug.Log("Should cancel tower here");
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
    }
}