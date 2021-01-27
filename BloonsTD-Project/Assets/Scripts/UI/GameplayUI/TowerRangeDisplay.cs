using UnityEngine;
using TMG.BloonsTD.Gameplay;

namespace TMG.BloonsTD.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TowerRangeDisplay : MonoBehaviour
    {
        [SerializeField] private Color _regularTowerRange;
        [SerializeField] private Color _invalidTowerRange;
        
        [SerializeField] private TowerPlacementController _towerPlacementController;
        [SerializeField] private TowerSelectionController _towerSelectionController;
        
        private SpriteRenderer _towerRangeIndicator;

        private void Awake()
        {
            _towerRangeIndicator = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _towerPlacementController.OnChangeRangeIndicator += ShowTowerRange;
            _towerPlacementController.OnHideRangeIndicator += HideTowerRange;
            _towerSelectionController.OnTowerSelected += ShowTowerRange;
            _towerSelectionController.OnTowerDeselected += HideTowerRange;
        }
        
        private void OnDisable()
        {
            _towerPlacementController.OnChangeRangeIndicator -= ShowTowerRange;
            _towerPlacementController.OnHideRangeIndicator -= HideTowerRange;
            _towerSelectionController.OnTowerSelected -= ShowTowerRange;
            _towerSelectionController.OnTowerDeselected -= HideTowerRange;
        }

        private void ShowTowerRange(TowerController towerController)
        {
            _towerRangeIndicator.color = _regularTowerRange;
        }
        
        private void ShowTowerRange(bool isValidPosition)
        {
            _towerRangeIndicator.color = isValidPosition ? _regularTowerRange : _invalidTowerRange;
        }

        private void HideTowerRange()
        {
            _towerRangeIndicator.color = Color.clear;
        }
    }
}