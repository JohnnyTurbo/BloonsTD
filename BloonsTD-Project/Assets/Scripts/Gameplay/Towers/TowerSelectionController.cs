using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerSelectionController : MonoBehaviour
    {
        public TowerController TowerController { get; set; }

        public delegate void TowerSelected(TowerController towerController);
        public event TowerSelected OnTowerSelected;

        public delegate void TowerDeselected();
        public event TowerDeselected OnTowerDeselected;

        public void SelectTower()
        {
            OnTowerSelected?.Invoke(TowerController);
        }

        public void DeselectTower()
        {
            OnTowerDeselected?.Invoke();
        }
    }
}