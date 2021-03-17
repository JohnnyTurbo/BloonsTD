using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class TowerBloonDetector : MonoBehaviour, IUpgradeRange
    {
        private TowerController _towerController;

        public TowerController TowerController
        {
            get => _towerController;
            set => _towerController = value;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.IsNotOnLayer(PhysicsLayers.Bloons)) { return; }
            _towerController.OnBloonEnter();
        }

        public void SetRange(float newRangeValue)
        {
            transform.localScale = Vector3.one * (newRangeValue * .01f);
        }
    }
}