using System;
using TMG.BloonsTD.Gameplay;
using UnityEngine;
using TMG.BloonsTD.Stats;
using TMPro;
using UnityEngine.EventSystems;
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
        [SerializeField] private int _testInt;
        
        private void Start()
        {
            HideTowerInformation();
            DisableTowerBuildButtons();
        }

        private void OnEnable()
        {
            GameController.Instance.OnGameBegin += EnableTowerBuildButtons;
            GameController.Instance.OnGameOver += DisableTowerBuildButtons;
            GameController.Instance.OnGameOver += HideTowerInformation;
        }

        private void OnDisable()
        {
            GameController.Instance.OnGameBegin -= EnableTowerBuildButtons;
            GameController.Instance.OnGameOver -= DisableTowerBuildButtons;
            GameController.Instance.OnGameOver -= HideTowerInformation;
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

        private void EnableTowerBuildButtons()
        {
            foreach (var towerBuildButton in _towerBuildButtons)
            {
                towerBuildButton.interactable = true;
                towerBuildButton.GetComponent<EventTrigger>().enabled = true;
            }
        }
        
        private void DisableTowerBuildButtons()
        {
            foreach (var towerBuildButton in _towerBuildButtons)
            {
                towerBuildButton.interactable = false;
                towerBuildButton.GetComponent<EventTrigger>().enabled = false;
            }
        }
    }
}