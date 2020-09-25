using System;
using TMG.BloonsTD.Controllers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TMG.BloonsTD.UI
{
    public class GeneralUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roundValue;
        [SerializeField] private TMP_Text _moneyValue;
        [SerializeField] private TMP_Text _livesValue;
        [SerializeField] private Button _startRoundButton;
        [SerializeField] private GameController _gameController;
        [SerializeField] private RoundController _roundController;

        private void OnEnable()
        {
            _gameController.OnRoundChanged += UpdateRoundValue;
            _gameController.OnMoneyChanged += UpdateMoneyValue;
            _gameController.OnLivesChanged += UpdateLivesValue;
            _roundController.OnRoundComplete += ShowStartRoundButton;
        }

        private void OnDisable()
        {
            _gameController.OnRoundChanged -= UpdateRoundValue;
            _gameController.OnMoneyChanged -= UpdateMoneyValue;
            _gameController.OnLivesChanged -= UpdateLivesValue;
            _roundController.OnRoundComplete -= ShowStartRoundButton;
        }

        private void UpdateRoundValue(string currentRound) =>
            _roundValue.text = currentRound.Equals(0.ToString()) ? 1.ToString() : currentRound;

        private void UpdateMoneyValue(string currentMoney) => _moneyValue.text = currentMoney;

        private void UpdateLivesValue(string currentLives) => _livesValue.text = currentLives;

        private void ShowStartRoundButton() => _startRoundButton.gameObject.SetActive(true);

        public void HideStartRoundButton() => _startRoundButton.gameObject.SetActive(false);
    }
}