using System;
using TMG.BloonsTD.Controllers;
using UnityEngine;

namespace TMG.BloonsTD.UI
{
    public class TowerRangeDisplay : MonoBehaviour
    {
        [SerializeField] private Color _regularTowerRange;
        [SerializeField] private Color _invalidTowerRange;
        private SpriteRenderer _towerRangeIndicator;
        private bool _showTowerRange;
        private TowerPlacementController _towerPlacementController;

        private void Awake()
        {
            _towerRangeIndicator = GetComponent<SpriteRenderer>();
            if (_towerRangeIndicator == null)
            {
                Debug.LogWarning("Warning, no SpriteRenderer on Tower Range Indicator. Adding default one", gameObject);
                _towerRangeIndicator = gameObject.AddComponent<SpriteRenderer>();
            }

            _towerPlacementController = GetComponentInParent<TowerPlacementController>();
            if (_towerPlacementController == null)
            {
                Debug.LogWarning("Warning, no TowerPlacementController found on parent.");
            }
        }

        private void Start()
        {
            _showTowerRange = true;
        }

        private void Update()
        {
            if (!_showTowerRange || _towerPlacementController == null) { return; }

            _towerRangeIndicator.color = _towerPlacementController.IsValidPlacementPosition
                ? _regularTowerRange
                : _invalidTowerRange;
        }
    }
}