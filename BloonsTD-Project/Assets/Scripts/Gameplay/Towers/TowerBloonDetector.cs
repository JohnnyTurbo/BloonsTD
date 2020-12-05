using TMG.BloonsTD.Helpers;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerBloonDetector : MonoBehaviour
    {
        private TowerController _towerController;
        private void Awake()
        {
            _towerController = GetComponentInParent<TowerController>();
            if (_towerController == null)
            {
                _towerController = transform.parent.gameObject.AddComponent<TowerController>();
                Debug.LogWarning("Warning no TowerController on parent. Adding default one", gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != BloonsReferences.BloonsLayer) { return; }
            _towerController.OnBloonEnter();
        }
    }
}