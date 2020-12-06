using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerSelectionController : MonoBehaviour
    {
        public TowerStatistics TowerStatistics { get; set; }

        public delegate void TowerSelected(TowerStatistics towerStatistics);
        public event TowerSelected OnTowerSelected;

        public delegate void TowerDeselected();
        public event TowerDeselected OnTowerDeselected;

        public void SelectTower()
        {
            OnTowerSelected?.Invoke(TowerStatistics);
        }

        public void DeselectTower()
        {
            OnTowerDeselected?.Invoke();
        }
    }
}