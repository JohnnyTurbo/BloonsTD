using TMG.BloonsTD.Gameplay;
using TMG.BloonsTD.Stats;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TMG.BloonsTD.UI
{
    public class GeneralUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roundText;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _livesText;
        [SerializeField] private Button _startRoundButton;
        [SerializeField] private GameController _gameController;
        [SerializeField] private RoundController _roundController;

        private void OnEnable()
        {
            _gameController.OnRoundChanged += UpdateRoundText;
            _gameController.OnMoneyChanged += UpdateMoneyText;
            _gameController.OnLivesChanged += UpdateLivesText;
            _gameController.OnGameBegin += ShowStartRoundButton;
            _roundController.OnRoundComplete += ShowStartRoundButton;
        }

        private void OnDisable()
        {
            _gameController.OnRoundChanged -= UpdateRoundText;
            _gameController.OnMoneyChanged -= UpdateMoneyText;
            _gameController.OnLivesChanged -= UpdateLivesText;
            _gameController.OnGameBegin -= ShowStartRoundButton;
            _roundController.OnRoundComplete -= ShowStartRoundButton;
        }

        private void Start()
        {
            HideStartRoundButton();
        }
        
        private void UpdateRoundText(int currentRound) =>
            _roundText.text = $"Round: {(currentRound.Equals(0) ? 1.ToString() : currentRound.ToString())}";
        private void UpdateMoneyText(int currentMoney) => _moneyText.text = $"Money: {currentMoney.ToString()}";
        private void UpdateLivesText(int currentLives) => _livesText.text = $"Lives: {currentLives.ToString()}";
        private void ShowStartRoundButton(RoundProperties round) => ShowStartRoundButton();
        private void ShowStartRoundButton() => _startRoundButton.gameObject.SetActive(true);
        public void HideStartRoundButton() => _startRoundButton.gameObject.SetActive(false);
    }
}