using System;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class DebugHelper : MonoBehaviour
    {
        [SerializeField] private float _speedMultiplier;
        
        private bool _isSpeedy;

#if (!UNITY_EDITOR)
        private void Awake()
        {
            Destroy(gameObject);
        }
#endif
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _isSpeedy = !_isSpeedy;
                Time.timeScale = _isSpeedy ? _speedMultiplier : 1f;
            }
        }
    }
}