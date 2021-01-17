using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class FreezeAttack : TowerAttack
    {
        private float _freezeDuration;

        public override void Initialize(TowerController towerController)
        {
            base.Initialize(towerController);
            _freezeDuration = TowerController.TowerProperties.FreezeDuration;
        }

        protected override void Attack(Vector3 targetLocation)
        {
            Debug.Log($"Applying status effect for {_freezeDuration}");
        }
    }
}