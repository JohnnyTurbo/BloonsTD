using System;
using TMG.BloonsTD.Gameplay;
using UnityEngine;
using TMG.BloonsTD.Stats;
using TMPro;
using UnityEngine.UI;

namespace TMG.BloonsTD.UI
{
    public class BuildTowersUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _towerInformationPanel;
        [SerializeField] private TMP_Text _towerName;
        [SerializeField] private TMP_Text _towerCost;
        [SerializeField] private TMP_Text _towerSpeed;
        [SerializeField] private TMP_Text _towerDescription;
        [SerializeField] private Button[] _towerBuildButtons;
        

        private void Start()
        {
            HideTowerInformation();
        }

        private void OnEnable()
        {
            GameController.Instance.OnGameOver += DisableTowerBuildButtons;
        }

        private void OnDisable()
        {
            GameController.Instance.OnGameOver -= DisableTowerBuildButtons;
        }

        public void ShowTowerInformation(TowerProperties towerProperties)
        {
            _towerInformationPanel.SetActive(true);

            _towerName.text = towerProperties.TowerName;
            _towerCost.text = $"<b>Cost:</b> {towerProperties.Cost.ToString()}";
            _towerSpeed.text = $"<b>Speed:</b> {towerProperties.Speed}";
            _towerDescription.text = towerProperties.Description;
        }

        public void HideTowerInformation()
        {
            _towerName.text = "";
            _towerCost.text = "";
            _towerSpeed.text = "";
            _towerDescription.text = "";
            
            _towerInformationPanel.SetActive(false);
        }

        private void DisableTowerBuildButtons()
        {
            foreach (var towerBuildButton in _towerBuildButtons)
            {
                towerBuildButton.interactable = false;
            }
        }
    }
}