using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class ProjectileAttack : TowerAttack, IUpgradeWeapon
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
            var newProjectile = Object.Instantiate(_projectile, TowerPosition, rotation);
            if (newProjectile.GetComponent<Hazard>() is IUpgradeRange newProjectileController)
                newProjectileController.SetRange(Range);
        }
        
        public void SetWeapon(GameObject newWeaponPrefab)
        {
            _projectile = newWeaponPrefab;
        }
    }
}