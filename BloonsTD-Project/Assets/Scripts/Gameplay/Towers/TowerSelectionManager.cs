using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [SerializeField] private TowerSpawnManager _towerSpawnManager;

        private TowerSelectionController _selectedTower;

        private void Update()
        {
            if(_towerSpawnManager.TowerPlacementState == TowerPlacementState.PlacingTower){ return; }

            if (InputController.BeginScreenSelection)
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

            if (InputController.BeginCancelSelection)
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