using System;
using TMG.BloonsTD.Helpers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [SerializeField] private TowerSpawnManager _towerSpawnManager;

        private TowerSelectionController _selectedTower;

        private void Update()
        {
            if(_towerSpawnManager.TowerPlacementState == TowerPlacementState.PlacingTower){ return; }

            if (InputController.ScreenSelectionFlag)
            {
                var worldSelectionPosition = InputController.WorldSelectionPosition;
                var selectedCollider = Physics2D.OverlapPoint(worldSelectionPosition, 1 << BloonsReferences.TowerLayer);
                if(selectedCollider != null)
                {
                    var selectionController = selectedCollider.GetComponent<TowerSelectionController>();
                    SelectTower(selectionController);
                }
                else
                {
                    DeselectTower();
                }
            }

            if (InputController.CancelSelection)
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

        private void DeselectTower()
        {
            if (_selectedTower == null) { return; }
            _selectedTower.DeselectTower();
        }
    }
}