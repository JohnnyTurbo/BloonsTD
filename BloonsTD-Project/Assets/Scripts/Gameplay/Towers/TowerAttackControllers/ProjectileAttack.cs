using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class ProjectileAttack : TowerAttack, IUpgradeRange, IUpgradeWeapon
    {
        private GameObject _projectile;

        public override void Initialize(TowerController towerController)
        {
            base.Initialize(towerController);
            _projectile = TowerController.TowerProperties.ProjectilePrefab;
        }

        protected override void Attack(Vector3 targetLocation)
        {
            var rotation = GetOrientationToTarget(TowerPosition, targetLocation);
            TowerController.transform.rotation = rotation;
            Object.Instantiate(_projectile, TowerPosition, rotation);
        }

        public void UpgradeRange(float newRangeValue)
        {
            DetectionCollider.radius = newRangeValue;
            //TODO: also change distance darts can go
        }

        public void UpgradeWeapon(GameObject newWeaponPrefab)
        {
            _projectile = newWeaponPrefab;
        }
    }
}