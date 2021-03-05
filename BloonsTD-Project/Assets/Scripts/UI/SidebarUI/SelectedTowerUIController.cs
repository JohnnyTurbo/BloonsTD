using System;
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
        
        [Header("Tower Info UI Elements")]
        [SerializeField] private TMP_Text _towerNameText;
        [SerializeField] private TMP_Text _towerSpeedText;
        [SerializeField] private TMP_Text _towerRangeText;
        [SerializeField] private Button _sellTowerButton;
        [SerializeField] private TMP_Text _sellTowerText;
        
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

        private UpgradeButtonDTO _upgradeButtonDTO1;
        private UpgradeButtonDTO _upgradeButtonDTO2;
        private TowerController _towerController;
        private TowerUpgrade _upgrade1;
        private TowerUpgrade _upgrade2;
        private void Awake()
        {
            SetupEvents();
        }

        private void Start()
        {
            _selectedTowerPanel.SetActive(false);
            _upgradeButtonDTO1 = new UpgradeButtonDTO
            {
                Button = _upgrade1Button,
                CostText = _upgrade1CostText,
                NameText = _upgrade1NameText,
                Icon = _upgrade1Icon
            };
            _upgradeButtonDTO2 = new UpgradeButtonDTO
            {
                Button = _upgrade2Button,
                CostText = _upgrade2CostText,
                NameText = _upgrade2NameText,
                Icon = _upgrade2Icon
            };
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
            _towerController = towerController;
            var towerProperties = _towerController.TowerProperties;
            
            _towerNameText.text = towerProperties.TowerName;
            _towerSpeedText.text = $"Speed: {towerProperties.Speed}";
            _towerRangeText.text = $"Range: {towerController.TowerRange}";
            _sellTowerButton.onClick.RemoveAllListeners();
            _sellTowerButton.onClick.AddListener(() => OnButtonSellTower(towerController));
            _sellTowerText.text = $"Sell for: {towerController.SellTowerCost}";
            
            var upgradeController = _towerController.TowerUpgradeController;
            _upgrade1 = upgradeController.Upgrades[0];
            _upgrade2 = upgradeController.Upgrades[1];
            
            SetupUpgradeButton(_upgrade1, _upgradeButtonDTO1);
            SetupUpgradeButton(_upgrade2, _upgradeButtonDTO2);
            _gameController.OnMoneyChanged += OnMoneyChanged;
        }

        private static void OnButtonSellTower(TowerController towerController)
        {
            towerController.SellTower();
        }
        
        private void HideTowerUI()
        {
            _towerController = null;
            _upgrade1 = null;
            _upgrade2 = null;
            _gameController.OnMoneyChanged -= OnMoneyChanged;
            _selectedTowerPanel.SetActive(false);
        }

        private void OnMoneyChanged(int newMoneyValue)
        {
            if(!_selectedTowerPanel.activeSelf) {return;}
            SetupUpgradeButton(_upgrade1, _upgradeButtonDTO1);
            SetupUpgradeButton(_upgrade2, _upgradeButtonDTO2);
        }
        
        private void SetupUpgradeButton(TowerUpgrade upgrade, UpgradeButtonDTO upgradeButtonDTO)
        {
            if (upgrade == null)
            {
                ShowBlankButton(upgradeButtonDTO);
                return;
            }
            var button = upgradeButtonDTO.Button;
            var nameText = upgradeButtonDTO.NameText;
            var costText = upgradeButtonDTO.CostText;
            var iconImage = upgradeButtonDTO.Icon;

            button.image.color = Color.white;
            iconImage.color = Color.white;
            nameText.text = upgrade.Name;
            iconImage.sprite = upgrade.Icon;
            button.onClick.RemoveAllListeners();
            if (HavePurchased(upgrade))
            {
                button.interactable = false;
                costText.text = $"Already Bought: {upgrade.Cost}";
            }
            else if (CannotAfford(upgrade))
            {
                button.interactable = false;
                costText.text = $"Can't Afford\n{upgrade.Cost}";
            }
            else
            {
                button.interactable = true;
                button.onClick.AddListener(() => OnButtonUpgradeTower(upgrade, _towerController));
                costText.text = $"Buy for: {upgrade.Cost}";
            }
        }

        private void ShowBlankButton(UpgradeButtonDTO upgradeButtonDTO)
        {
            var button = upgradeButtonDTO.Button;
            var nameText = upgradeButtonDTO.NameText;
            var costText = upgradeButtonDTO.CostText;
            var iconImage = upgradeButtonDTO.Icon;
            
            button.image.color = Color.clear;
            button.interactable = false;
            nameText.text = string.Empty;
            costText.text = string.Empty;
            iconImage.color = Color.clear;
        }

        private static bool HavePurchased(TowerUpgrade upgrade)
        {
            return upgrade.IsPurchased;
        }

        private bool CannotAfford(TowerUpgrade upgrade)
        {
            return _gameController.Money < upgrade.Cost;
        }

        private static void OnButtonUpgradeTower(TowerUpgrade towerUpgrade, TowerController towerController)
        {
            towerUpgrade.PurchaseUpgrade(towerController);
            towerController.SelectionController.SelectTower();
        }

        private class UpgradeButtonDTO
        {
            public Button Button;
            public TMP_Text NameText;
            public TMP_Text CostText;
            public Image Icon;
        }
    }
}