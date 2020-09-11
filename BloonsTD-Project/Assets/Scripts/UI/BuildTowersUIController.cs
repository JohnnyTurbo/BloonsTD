using System;
using UnityEngine;
using TMG.BloonsTD.Stats;
using TMPro;

namespace TMG.BloonsTD.UI
{
    public class BuildTowersUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _towerInformationPanel;
        [SerializeField] private TMP_Text _towerName;
        [SerializeField] private TMP_Text _towerCost;
        [SerializeField] private TMP_Text _towerSpeed;
        [SerializeField] private TMP_Text _towerDescription;

        private void Start()
        {
            HideTowerInformation();
        }

        public void ShowTowerInformation(TowerInformation towerInformation)
        {
            _towerInformationPanel.SetActive(true);

            _towerName.text = towerInformation.Name;
            _towerCost.text = $"<b>Cost:</b> {towerInformation.Cost.ToString()}";
            _towerSpeed.text = $"<b>Speed:</b> {towerInformation.Speed}";
            _towerDescription.text = towerInformation.Description;
        }

        public void HideTowerInformation()
        {
            _towerName.text = "";
            _towerCost.text = "";
            _towerSpeed.text = "";
            _towerDescription.text = "";
            
            _towerInformationPanel.SetActive(false);
        }
    }
}