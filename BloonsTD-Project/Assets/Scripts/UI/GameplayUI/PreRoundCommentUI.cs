using System;
using System.Collections;
using TMG.BloonsTD.Gameplay;
using TMG.BloonsTD.Stats;
using TMPro;
using UnityEngine;

namespace TMG.BloonsTD.UI
{
    public class PreRoundCommentUI : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogueBox;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private float _displayTime;
        [SerializeField] private float _fadeTime;
        
        
        private void OnEnable()
        {
            _roundController.OnQueueNextRound += DisplayPreRoundComment;
        }

        private void OnDisable()
        {
            _roundController.OnQueueNextRound -= DisplayPreRoundComment;
        }

        private void DisplayPreRoundComment(RoundProperties nextRound)
        {
            _dialogueBox.gameObject.SetActive(true);
            _text.text = nextRound.PreRoundComment;
            StartCoroutine(HidePreRoundComment());
        }

        private IEnumerator HidePreRoundComment()
        {
            yield return new WaitForSeconds(_displayTime);
            _dialogueBox.gameObject.SetActive(false);
        }
    }
}