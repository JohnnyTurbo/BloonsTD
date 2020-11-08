using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerController : MonoBehaviour
    {
        private TowerProperties _towerProperties;
        private TowerStatistics _towerStatistics;
        private TowerPlacementController _placement;
        private TowerSelectionController _selectionController;
        public TowerProperties TowerProperties => _towerProperties;
        public TowerStatistics TowerStatistics => _towerStatistics;
        public TowerPlacementController Placement => _placement;
        public TowerSelectionController SelectionController => _selectionController;

        private void Awake()
        {
            _placement = GetComponent<TowerPlacementController>();
            if (_placement == null)
            {
                _placement = gameObject.AddComponent<TowerPlacementController>();
            }

            _selectionController = GetComponent<TowerSelectionController>();
        }

        public void InitializeTower(TowerProperties towerProperties)
        {
            _towerProperties = towerProperties;
            _towerStatistics = ScriptableObject.CreateInstance<TowerStatistics>();
            _towerStatistics.Set(towerProperties);
            _placement.TowerProperties = towerProperties;
            _selectionController.TowerStatistics = _towerStatistics;
            //TODO: Set renderer, collider, etc.
        }
    }
}