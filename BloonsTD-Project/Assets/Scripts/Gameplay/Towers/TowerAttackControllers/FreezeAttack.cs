using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class FreezeAttack : TowerAttack, IUpgradeDuration
    {
        private float _freezeDuration;
        private Vector3 _towerPosition;
        
        public override void Initialize(TowerController towerController)
        {
            base.Initialize(towerController);
            TowerTargetType = TowerTargetType.NoTarget;
            _freezeDuration = TowerController.TowerProperties.FreezeDuration;
            _towerPosition = towerController.transform.position;
        }

        protected override void Attack(Vector3 targetLocation)
        {
            var bloonCollidersInRange = new List<Collider2D>();
            var bloonFilter = new ContactFilter2D {layerMask = 1 << PhysicsLayers.Bloons, useLayerMask = true};
            Physics2D.OverlapCircle(_towerPosition, Range, bloonFilter, bloonCollidersInRange);
            foreach (var bloonCollider in bloonCollidersInRange)
            {
                var bloonController = bloonCollider.GetComponent<BloonController>();
                if (!bloonController.BloonProperties.CanBeFrozen || bloonController.IsFrozen) continue;
                bloonController.FreezeBloon(_freezeDuration);
            }
        }

        public void SetDuration(float newDurationValue)
        {
            _freezeDuration = newDurationValue;
        }
    }
}