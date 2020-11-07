using System;
using TMG.BloonsTD.Helpers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [SerializeField] private TowerSpawnManager _towerSpawnManager;

        //private 
        
        private void Update()
        {
            if(_towerSpawnManager.TowerPlacementState == TowerPlacementState.PlacingTower){ return; }

            if (InputController.ScreenSelectionFlag)
            {
                var worldSelectionPosition = InputController.WorldSelectionPosition;
                var selectedCollider = Physics2D.OverlapPoint(worldSelectionPosition, 1 << BloonsReferences.TowerLayer);
                var selectedTower = selectedCollider.GetComponent<TowerProperties>();
                SelectTower(selectedTower);
            }
        }

        private void SelectTower(TowerProperties selectedTower)
        {
            
        }
    }
}