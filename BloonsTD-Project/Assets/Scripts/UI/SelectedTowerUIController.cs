using System;
using TMG.BloonsTD.Controllers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.UI
{
    public class SelectedTowerUIController : MonoBehaviour
    {
        [SerializeField] private TowerSpawnManager _towerSpawnManager;

        private void Awake()
        {
            SetupEvents();
        }

        private void SetupEvents()
        {
            _towerSpawnManager.OnTowerPlaced += SetupTowerEvents;
        }

        private void SetupTowerEvents(TowerController towerController)
        {
            towerController.SelectionController.OnTowerSelected += DisplaySelectedTowerUI;
        }
        
        private void DisplaySelectedTowerUI(TowerStatistics towerStatistics)
        {
            //Debug.Log($"Upgrade 1: {towerStatistics.TowerUpgradePath1.Name} costs: {towerStatistics.TowerUpgradePath1.Cost}");
            //Debug.Log($"Upgrade 2: {towerStatistics.TowerUpgradePath2.Name} costs: {towerStatistics.TowerUpgradePath2.Cost}");
        }
    }
}