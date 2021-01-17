using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public abstract class TowerAttack
    {
        private WaitForSeconds _attackCooldownTime;
        protected CircleCollider2D DetectionCollider;
        protected TowerController TowerController;
        protected Vector3 TowerPosition;
        protected TowerTargetType TowerTargetType;
        public static TowerAttack GetNewAttackController(TowerAttackType towerAttackType)
        {
            switch (towerAttackType)
            {
                case TowerAttackType.Projectile:
                    var projectileAttackController = new ProjectileAttack();
                    return projectileAttackController;
                case TowerAttackType.MultiProjectile:
                    var multiProjectileAttackController = new MultiProjectileAttack();
                    return multiProjectileAttackController;
                case TowerAttackType.Freeze:
                    var freezeAttackController = new FreezeAttack();
                    return freezeAttackController;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void Initialize(TowerController towerController)
        {
            TowerController = towerController;
            TowerPosition = TowerController.transform.position;
            _attackCooldownTime = new WaitForSeconds(towerController.TowerProperties.AttackCooldownTime);
            TowerTargetType = TowerTargetType.First;
            DetectionCollider = TowerController.transform.Find("DetectionRadius").GetComponent<CircleCollider2D>();
            if (DetectionCollider == null)
            {
                Debug.LogWarning("Warning: could not get Collider2D component on the DetectionRadius GameObject.", TowerController);
            }
        }

        public bool TryAttack()
        {
            var bloonCollidersInRange = new List<Collider2D>();
            var bloonFilter = new ContactFilter2D {layerMask = 1 << PhysicsLayers.Bloons, useLayerMask = true};
            var numBloonsInRange = DetectionCollider.OverlapCollider(bloonFilter, bloonCollidersInRange);
            if (numBloonsInRange <= 0)
            {
                return false;
            }
            var bloonsInRange = bloonCollidersInRange.Select(bloon => bloon.GetComponent<BloonController>()).ToList();
            var attackTargetLocation = DetermineTargetLocation(bloonsInRange);
            Attack(attackTargetLocation);
            TowerController.StartCooldownTimer(_attackCooldownTime);
            return true;
        }

        private Vector3 DetermineTargetLocation(IReadOnlyList<BloonController> bloons)
        {
            switch (TowerTargetType)
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
            var furthestBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                furthestBloon = BloonController.CompareGreaterPathProgress(furthestBloon, bloons[i]);
            }
            return furthestBloon.transform.position;
        }

        private static Vector3 GetLastBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            var lastBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                lastBloon = BloonController.CompareLeastPathProgress(lastBloon, bloons[i]);
            }
            return lastBloon.transform.position;
        }

        private static Vector3 GetStrongestBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            var strongestBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                strongestBloon = BloonController.CompareStrongest(strongestBloon, bloons[i]);
            }
            return strongestBloon.transform.position;
        }

        private static Vector3 GetWeakestBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            var weakestBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                weakestBloon = BloonController.CompareWeakest(weakestBloon, bloons[i]);
            }
            return weakestBloon.transform.position;
        }

        private Vector3 GetClosestBloonPosition(IReadOnlyList<BloonController> bloons)
        {
            var closestBloon = bloons[0];
            var closestDistance = Vector3.Distance(TowerPosition, closestBloon.transform.position);
            for (var i = 1; i < bloons.Count; i++)
            {
                var currentBloonDistance = Vector3.Distance(TowerPosition, bloons[i].transform.position);
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