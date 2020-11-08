using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerSelectionController : MonoBehaviour
    {
        public delegate void TowerSelection();

        public event TowerSelection OnTowerSelected;
        public event TowerSelection OnTowerDeselected;

        public void SelectTower()
        {
            Debug.Log("SELECT TOWER");
            OnTowerSelected?.Invoke();
        }

        public void DeselectTower()
        {
            OnTowerDeselected?.Invoke();
        }
    }
}