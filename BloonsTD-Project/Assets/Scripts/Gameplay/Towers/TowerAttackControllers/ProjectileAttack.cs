using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class ProjectileAttack : TowerAttack
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
            GameObject.Instantiate(_projectile, TowerPosition, rotation);
        }
    }
}