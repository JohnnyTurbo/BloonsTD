using System;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class DebugHelper : MonoBehaviour
    {
        private bool _isSpeedy = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _isSpeedy = !_isSpeedy;
                Time.timeScale = _isSpeedy ? 4f : 1f;
            }
        }
    }
}