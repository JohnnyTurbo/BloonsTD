using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay.TowerAttackControllers
{
    [RequireComponent(typeof(TowerController))]
    public class TowerAttackController : MonoBehaviour
    {
        private Collider2D _detectionCollider;
        protected TowerController _towerController;
        private void Awake()
        {
            _detectionCollider = transform.Find("DetectionRadius").GetComponent<Collider2D>();
            if (_detectionCollider == null)
            {
                Debug.LogWarning("Warning: could not get Collider2D component on the DetectionRadius GameObject.", gameObject);
            }
            _towerController = GetComponent<TowerController>();
        }

        public bool TryAttack()
        {
            var bloonCollidersInRange = new List<Collider2D>();
            var bloonFilter = new ContactFilter2D {layerMask = 1 << PhysicsLayers.Bloons, useLayerMask = true};
            var numBloonsInRange = _detectionCollider.OverlapCollider(bloonFilter, bloonCollidersInRange);
            if (numBloonsInRange <= 0)
            {
                return false;
            }
            var bloonsInRange = bloonCollidersInRange.Select(bloon => bloon.GetComponent<BloonController>()).ToList();
            var attackTargetLocation = DetermineTargetLocation(bloonsInRange);
            Attack(attackTargetLocation);
            return true;
        }

        private Vector3 DetermineTargetLocation(IReadOnlyList<BloonController> bloons)
        {
            switch (_towerController.TowerTargetType)
            {
                case TowerTargetType.First:
                    return GetFirstBloonPosition(bloons);
                case TowerTargetType.Last:
                    return GetLastBloonPosition(bloons);
                case TowerTargetType.Strongest:
                    return GetStrongestBloonPosition(bloons);
                case TowerTargetType.Weakest:
                    return GetWeakestBloonPosition(bloons);
                case TowerTargetType.Closest:
                    return GetClosestBloonPosition(bloons);
                case TowerTargetType.NoTarget:
                    return Vector3.zero;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Vector3 GetFirstBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            BloonController furthestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                furthestBloon = BloonController.CompareGreaterPathProgress(furthestBloon, bloons[i]);
            }
            return furthestBloon.transform.position;
        }

        private static Vector3 GetLastBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            BloonController lastBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                lastBloon = BloonController.CompareLeastPathProgress(lastBloon, bloons[i]);
            }
            return lastBloon.transform.position;
        }

        private static Vector3 GetStrongestBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            BloonController strongestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                strongestBloon = BloonController.CompareStrongest(strongestBloon, bloons[i]);
            }
            return strongestBloon.transform.position;
        }

        private static Vector3 GetWeakestBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            BloonController weakestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                weakestBloon = BloonController.CompareWeakest(weakestBloon, bloons[i]);
            }
            return weakestBloon.transform.position;
        }

        private Vector3 GetClosestBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            var towerPosition = transform.position;
            var closestBloon = bloons[0];
            var closestDistance = Vector3.Distance(towerPosition, closestBloon.transform.position);
            for (int i = 1; i < bloons.Count; i++)
            {
                var currentBloonDistance = Vector3.Distance(towerPosition, bloons[i].transform.position);
                var currentBloonIsCloserThanPreviousClosest = currentBloonDistance < closestDistance;
                
                if (!currentBloonIsCloserThanPreviousClosest) continue;
                
                closestBloon = bloons[i];
                closestDistance = currentBloonDistance;
            }
            return closestBloon.transform.position;
        }

        protected static Quaternion GetOrientationToTarget(Vector3 position, Vector3 targetLocation)
        {
            var towerToTarget = targetLocation - position;
            var angle = Vector3.SignedAngle(Vector3.up, towerToTarget, Vector3.forward);
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            return rotation;
        }
        
        protected virtual void Attack(Vector3 targetLocation)
        {
        }
    }
}