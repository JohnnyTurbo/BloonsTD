using UnityEngine;

namespace TMG.BloonsTD.Gameplay.TowerAttackControllers
{
    public class ProjectileAttackController : TowerAttackController
    {
        private GameObject _projectile;

        private void Start()
        {
            _projectile = _towerController.ProjectilePrefab;
        }

        protected override void Attack(Vector3 targetLocation)
        {
            var position = transform.position;
            var rotation = GetOrientationToTarget(position, targetLocation);
            transform.rotation = rotation;
            Instantiate(_projectile, position, rotation);
        }
        
        
    }
}