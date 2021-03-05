using System;
using System.Collections;
using TMG.BloonsTD.Gameplay;
using TMG.BloonsTD.Stats;
using TMPro;
using UnityEngine;

namespace TMG.BloonsTD.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogueBox;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private TowerSpawnManager _towerSpawnManager;
        [SerializeField] private float _displayTime;
        [SerializeField] private float _fadeTime;

        private Coroutine _currentMessageTimer;
        private void OnEnable()
        {
            _roundController.OnQueueNextRound += DisplayPreRoundComment;
            _towerSpawnManager.OnDisplayMessage += DisplayDialogue;
        }

        private void OnDisable()
        {
            _roundController.OnQueueNextRound -= DisplayPreRoundComment;
            _towerSpawnManager.OnDisplayMessage -= DisplayDialogue;
        }

        private void DisplayPreRoundComment(RoundProperties nextRound)
        {
            var rewardText = nextRound.RoundNumber == 1 ? "" :
                $"Round {nextRound.RoundNumber - 1} passed. {102 - nextRound.RoundNumber} money awarded. ";
            DisplayDialogue(rewardText + nextRound.PreRoundComment);
        }
        private void DisplayDialogue(string textToDisplay)
        {
            if (_currentMessageTimer != null)
            {
                StopCoroutine(_currentMessageTimer);
            }
            _dialogueBox.gameObject.SetActive(true);
            _text.text = textToDisplay;
            _currentMessageTimer = StartCoroutine(HideDialogue());
        }
        
        private IEnumerator HideDialogue()
        {
            yield return new WaitForSeconds(_displayTime);
            _dialogueBox.gameObject.SetActive(false);

            _currentMessageTimer = null;
        }
    }
}