using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay.TowerAttackControllers
{
    public class TowerAttackController : MonoBehaviour
    {
        private Collider2D _attackRadius;
        protected TowerController _towerController;
        
        private void Awake()
        {
            _attackRadius = transform.Find("DetectionRadius").GetComponent<Collider2D>();
            _towerController = GetComponent<TowerController>();
        }

        public bool TryAttack()
        {
            var bloonCollidersInRange = new List<Collider2D>();
            var bloonFilter = new ContactFilter2D {layerMask = 1 << BloonsReferences.BloonsLayer, useLayerMask = true};
            var numBloonsInRange = _attackRadius.OverlapCollider(bloonFilter, bloonCollidersInRange);
            if (numBloonsInRange <= 0)
            {
                return false;
            }
            var bloonsInRange = bloonCollidersInRange.Select(bloon => bloon.GetComponent<BloonController>()).ToList();
            var targetLocation = DetermineTarget(bloonsInRange);
            Attack(targetLocation);
            return true;
        }

        private Vector3 DetermineTarget(List<BloonController> bloons)
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

        private Vector3 GetFirstBloonPosition(List<BloonController> bloons)
        {
            BloonController furthestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                furthestBloon = BloonController.CompareGreaterPathProgress(furthestBloon, bloons[i]);
            }
            return furthestBloon.transform.position;
        }

        private Vector3 GetLastBloonPosition(List<BloonController> bloons)
        {
            BloonController lastBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                lastBloon = BloonController.CompareLeastPathProgress(lastBloon, bloons[i]);
            }
            return lastBloon.transform.position;
        }

        private Vector3 GetStrongestBloonPosition(List<BloonController> bloons)
        {
            BloonController strongestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                strongestBloon = BloonController.CompareStrongest(strongestBloon, bloons[i]);
            }
            return strongestBloon.transform.position;
        }

        private Vector3 GetWeakestBloonPosition(List<BloonController> bloons)
        {
            BloonController weakestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                weakestBloon = BloonController.CompareWeakest(weakestBloon, bloons[i]);
            }
            return weakestBloon.transform.position;
        }

        private Vector3 GetClosestBloonPosition(List<BloonController> bloons)
        {
            var towerPosition = transform.position;
            var closestBloon = bloons[0];
            var closestDistance = Vector3.Distance(towerPosition, closestBloon.transform.position);
            for (int i = 1; i < bloons.Count; i++)
            {
                var currentBloonDistance = Vector3.Distance(towerPosition, bloons[i].transform.position);
                if (currentBloonDistance < closestDistance)
                {
                    closestBloon = bloons[i];
                    closestDistance = currentBloonDistance;
                }
            }
            return closestBloon.transform.position;
        }

        protected virtual void Attack(Vector3 targetLocation)
        {
        }
    }
}