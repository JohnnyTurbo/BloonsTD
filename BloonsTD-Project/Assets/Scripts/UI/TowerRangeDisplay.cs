using System;
using TMG.BloonsTD.Controllers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.UI
{
    public class TowerRangeDisplay : MonoBehaviour
    {
        [SerializeField] private Color _regularTowerRange;
        [SerializeField] private Color _invalidTowerRange;
        [SerializeField] private TowerPlacementController _towerPlacementController;
        [SerializeField] private TowerSelectionController _towerSelectionController;
        //[SerializeField] private TowerProperties _towerProperties;
        
        private SpriteRenderer _towerRangeIndicator;
        private bool _showTowerRange;

        private void Awake()
        {
            _towerRangeIndicator = GetComponent<SpriteRenderer>();
            if (_towerRangeIndicator == null)
            {
                Debug.LogWarning("Warning, no SpriteRenderer on Tower Range Indicator. Adding default one", gameObject);
                _towerRangeIndicator = gameObject.AddComponent<SpriteRenderer>();
            }

            if (_towerPlacementController == null)
            {
                Debug.LogWarning("Warning, no TowerPlacementController set", gameObject);
            }
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

        private void ShowTowerRange()
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