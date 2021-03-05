using System;
using System.Collections.Generic;
using System.Linq;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public abstract class TowerAttack : IUpgradeRange, IUpgradeFrequency
    {
        private WaitForSeconds _attackCooldownTime;
        private CircleCollider2D _detectionCollider;
        private GameObject _detectionRadiusGO;
        private TowerBloonDetector _towerBloonDetector;
        protected TowerController TowerController;
        protected Vector3 TowerPosition;
        protected TowerTargetType TowerTargetType;
        public float Range { get; private set; }

        public static TowerAttack GetNewFromAttackType(TowerAttackType towerAttackType)
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
            _detectionRadiusGO = TowerController.transform.Find("DetectionRadius").gameObject;
            _detectionCollider = _detectionRadiusGO.GetComponent<CircleCollider2D>();
            _towerBloonDetector = _detectionRadiusGO.GetComponent<TowerBloonDetector>();
            if (_detectionCollider == null)
            {
                Debug.LogWarning("Warning: could not get Collider2D component on the DetectionRadius GameObject.",
                    TowerController);
            }
            SetRange(TowerController.TowerProperties.Range);
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
            TowerController.StartCooldownTimer(_attackCooldownTime);
            return true;
        }

        public void SetRange(float newRangeValue)
        {
            Range = newRangeValue;
            _towerBloonDetector.SetRange(newRangeValue);
        }

        public void SetFrequency(float newCooldownTime)
        {
            _attackCooldownTime = new WaitForSeconds(newCooldownTime);
        }

        private Vector3 DetermineTargetLocation(IReadOnlyList<BloonController> bloons)
        {
            switch (TowerTargetType)
            {
                case TowerTargetType.First:
                    return GetBloonPosition.First(bloons);
                case TowerTargetType.Last:
                    return GetBloonPosition.Last(bloons);
                case TowerTargetType.Strongest:
                    return GetBloonPosition.Strongest(bloons);
                case TowerTargetType.Weakest:
                    return GetBloonPosition.Weakest(bloons);
                case TowerTargetType.Closest:
                    return GetBloonPosition.Closest(bloons, TowerPosition);
                case TowerTargetType.NoTarget:
                    return Vector3.zero;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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