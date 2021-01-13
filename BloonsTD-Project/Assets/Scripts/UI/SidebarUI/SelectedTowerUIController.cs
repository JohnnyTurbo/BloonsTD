using System;
using NUnit.Framework.Internal;
using TMG.BloonsTD.Gameplay;
using TMG.BloonsTD.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMG.BloonsTD.UI
{
    public class SelectedTowerUIController : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private GameController _gameController;
        [SerializeField] private TowerSpawnManager _towerSpawnManager;
        [SerializeField] private GameObject _selectedTowerPanel;
        [Header("Upgrade General")]
        [SerializeField] private TMP_Text _towerNameText;
        [SerializeField] private TMP_Text _towerSpeedText;
        [SerializeField] private TMP_Text _towerRangeText;
        [Header("Upgrade 1")]
        [SerializeField] private Button _upgrade1Button;
        [SerializeField] private Image _upgrade1Icon;
        [SerializeField] private TMP_Text _upgrade1NameText;
        [SerializeField] private TMP_Text _upgrade1CostText;
        [Header("Upgrade 2")]
        [SerializeField] private Button _upgrade2Button;
        [SerializeField] private Image _upgrade2Icon;
        [SerializeField] private TMP_Text _upgrade2NameText;
        [SerializeField] private TMP_Text _upgrade2CostText;

        private void Awake()
        {
            SetupEvents();
        }

        private void Start()
        {
            _selectedTowerPanel.SetActive(false);
        }

        private void SetupEvents()
        {
            _towerSpawnManager.OnTowerPlaced += SetupTowerEvents;
        }

        private void SetupTowerEvents(TowerController towerController)
        {
            towerController.SelectionController.OnTowerSelected += DisplaySelectedTowerUI;
            towerController.SelectionController.OnTowerDeselected += HideTowerUI;
        }

        private void DisplaySelectedTowerUI(TowerController towerController)
        {
            _selectedTowerPanel.SetActive(true);
            DisplayTowerInformation(towerController);
            SetupUpgradeButtons(towerController);
            //Debug.Log($"Upgrade 1: {towerStatistics.TowerUpgradePath1.Name} costs: {towerStatistics.TowerUpgradePath1.Cost}");
            //Debug.Log($"Upgrade 2: {towerStatistics.TowerUpgradePath2.Name} costs: {towerStatistics.TowerUpgradePath2.Cost}");
        }

        private void HideTowerUI()
        {
            //_selectedTowerPanel.SetActive(false);
        }

        private void DisplayTowerInformation(TowerController towerController)
        {
            _towerNameText.text = towerController.TowerProperties.TowerName;
            _towerSpeedText.text = $"Speed: {towerController.AttackCooldownTime.ToString()}";
            _towerRangeText.text = $"Range: {towerController.AttackRange.ToString()}";
        }

        private void SetupUpgradeButtons(TowerController towerController)
        {
            _upgrade1NameText.text = towerController.Upgrades[0].Name;
            if (towerController.Upgrades[0].HasPurchased)
            {
                _upgrade1Button.interactable = false;
                _upgrade1CostText.text = $"Already Bought {towerController.Upgrades[0].Cost}";
            }
            else
            {
                _upgrade1Button.interactable = true;
                _upgrade1Button.onClick.AddListener(()=>UpgradeTower(towerController.Upgrades[0]));
                _upgrade1CostText.text = SetUpgradeCostText(_upgrade1CostText, towerController.Upgrades[0].Cost);
            }
            
            _upgrade2CostText.text = towerController.Upgrades[1].Name;
            if (towerController.Upgrades[1].HasPurchased)
            {
                _upgrade2Button.interactable = false;
                _upgrade2CostText.text = $"Already Bought {towerController.Upgrades[1].Cost}";
            }
            else
            {
                _upgrade2Button.interactable = true;
                _upgrade2Button.onClick.AddListener(()=>UpgradeTower(towerController.Upgrades[1]));
                _upgrade2CostText.text = SetUpgradeCostText(_upgrade1CostText, towerController.Upgrades[1].Cost);
            }
        }

        private void UpgradeTower(TowerUpgrade upgradeProperties)
        {
            Debug.Log("BUYIN!");
            upgradeProperties.PurchaseUpgrade();
        }

        private string SetUpgradeCostText(TMP_Text upgradeCostText, int upgradeCost)
        {
            return upgradeCost <= _gameController.Money ? $"Buy for: {upgradeCost}" : $"Can't Afford\n{upgradeCost}";
        }
    }
}