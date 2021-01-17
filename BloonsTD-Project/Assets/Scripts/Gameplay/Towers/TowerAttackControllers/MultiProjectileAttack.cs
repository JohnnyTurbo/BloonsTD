using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class MultiProjectileAttack : TowerAttack
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
            Debug.Log($"Shooting {_numberProjectiles} {_projectile.name}(s)");
        }
    }
}