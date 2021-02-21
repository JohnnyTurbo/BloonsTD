using System;
using TMG.BloonsTD.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MenuUI
{
    public class EndScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _endScreenPanel;
        [SerializeField] private TextMeshProUGUI _resultText;
        
        private void Start()
        {
            _endScreenPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameController.Instance.OnGameOver += ShowGameOverScreen;
        }

        private void OnDisable()
        {
            GameController.Instance.OnGameOver -= ShowGameOverScreen;
        }

        private void ShowGameOverScreen()
        {
            _endScreenPanel.SetActive(true);
            _resultText.text = GameController.Instance.Victory ? "CONGRATULATIONS!" : "GAME OVER!";
        }

        public void OnButtonRetry()
        {
            //TODO: Make without reloading level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnButtonPlayMoreGames()
        {
            Debug.Log("Play More Games!");
        }
    }
}