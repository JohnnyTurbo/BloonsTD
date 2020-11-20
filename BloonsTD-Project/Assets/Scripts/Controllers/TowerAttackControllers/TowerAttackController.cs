using System;
using System.Collections.Generic;
using TMG.BloonsTD.Helpers;
using UnityEngine;

namespace TMG.BloonsTD.Controllers.TowerAttackControllers
{
    public class TowerAttackController : MonoBehaviour
    {
        private Collider2D _attackRadius;

        private void Awake()
        {
            _attackRadius = transform.Find("DetectionRadius").GetComponent<Collider2D>();
        }

        public bool TryAttack()
        {
            var bloonsInRange = new List<Collider2D>();
            var bloonFilter = new ContactFilter2D {layerMask = 1 << BloonsReferences.BloonsLayer};
            var numBloonsInRange = _attackRadius.OverlapCollider(bloonFilter, bloonsInRange);
            if (numBloonsInRange <= 0)
            {
                return false;
            }

            var targetLocation = DetermineTarget(bloonsInRange);
            Attack(targetLocation);
            return true;
        }

        private Vector3 DetermineTarget<T>(List<T> bloons) where T : Component
        {
            var targetPosition = bloons[0].transform.position;
            return targetPosition;
        }

        protected virtual void Attack(Vector3 targetLocation)
        {
            Debug.Log("Base Attack");
        }
    }
}