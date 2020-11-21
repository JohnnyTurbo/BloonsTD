using System;
using System.Collections.Generic;
using System.Linq;
using TMG.BloonsTD.Helpers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers.TowerAttackControllers
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
                if (bloons[i].TargetWaypointIndex > furthestBloon.TargetWaypointIndex)
                {
                    furthestBloon = bloons[i];
                }
                else if (bloons[i].TargetWaypointIndex == furthestBloon.TargetWaypointIndex &&
                         bloons[i].PercentToNextWaypoint > furthestBloon.PercentToNextWaypoint)
                {
                    furthestBloon = bloons[i];
                }
            }
            return furthestBloon.transform.position;
        }

        private Vector3 GetLastBloonPosition(List<BloonController> bloons)
        {
            BloonController lastBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                if (bloons[i].TargetWaypointIndex < lastBloon.TargetWaypointIndex)
                {
                    lastBloon = bloons[i];
                }
                else if (bloons[i].TargetWaypointIndex == lastBloon.TargetWaypointIndex &&
                         bloons[i].PercentToNextWaypoint < lastBloon.PercentToNextWaypoint)
                {
                    lastBloon = bloons[i];
                }
            }
            return lastBloon.transform.position;
        }

        private Vector3 GetStrongestBloonPosition(List<BloonController> bloons)
        {
            BloonController strongestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                if (bloons[i].RBE > strongestBloon.RBE)
                {
                    strongestBloon = bloons[i];
                }
            }
            return strongestBloon.transform.position;
        }

        private Vector3 GetWeakestBloonPosition(List<BloonController> bloons)
        {
            BloonController weakestBloon = bloons[0];
            for (int i = 1; i < bloons.Count; i++)
            {
                if (bloons[i].RBE < weakestBloon.RBE)
                {
                    weakestBloon = bloons[i];
                }
            }
            return weakestBloon.transform.position;
        }

        protected virtual void Attack(Vector3 targetLocation)
        {
            //Debug.Log("Base Attack");
        }
    }
}