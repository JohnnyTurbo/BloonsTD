using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class MultiProjectileAttack : TowerAttack, IUpgradeWeapon
    {
        private GameObject _projectile;
        private int _numberProjectiles;
        public override void Initialize(TowerController towerController)
        {
            base.Initialize(towerController);
            _projectile = TowerController.TowerProperties.ProjectilePrefab;
            _numberProjectiles = TowerController.TowerProperties.NumberProjectiles;
        }

        protected override void Attack(Vector3 targetLocation)
        {
            if (targetLocation != Vector3.zero)
            {
                var towerRotation = GetOrientationToTarget(TowerPosition, targetLocation);
                TowerController.transform.rotation = towerRotation;
            }

            for (var i = 0; i < _numberProjectiles; i++)
            {
                var projectileRotationDeg = (360 / _numberProjectiles) * i;
                var projectileRotation = Quaternion.AngleAxis(projectileRotationDeg, Vector3.back);
                var newProjectile = Object.Instantiate(_projectile, TowerPosition, projectileRotation);
                var newProjectileController = newProjectile.GetComponent<BasicProjectileController>();
                newProjectileController.SetRange(Range);
            }
        }

        public void SetWeapon(GameObject newWeaponPrefab)
        {
            _projectile = newWeaponPrefab;
        }
    }
}