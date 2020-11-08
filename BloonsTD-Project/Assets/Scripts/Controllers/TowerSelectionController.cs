using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerSelectionController : MonoBehaviour
    {
        private TowerStatistics _towerStatistics;
        public TowerStatistics TowerStatistics
        {
            get => _towerStatistics;
            set => _towerStatistics = value;
        }
        public delegate void TowerSelected(TowerStatistics towerStatistics);
        public event TowerSelected OnTowerSelected;

        public delegate void TowerDeselected();
        public event TowerDeselected OnTowerDeselected;

        public void SelectTower()
        {
            OnTowerSelected?.Invoke(_towerStatistics);
        }

        public void DeselectTower()
        {
            OnTowerDeselected?.Invoke();
        }
    }
}