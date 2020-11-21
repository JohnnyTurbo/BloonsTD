using System.Collections.Generic;
using TMG.BloonsTD.Helpers;
using UnityEngine;

namespace TMG.BloonsTD.Controllers.TowerAttackControllers
{
    public class ProjectileAttackController : TowerAttackController
    {
        protected override void Attack(Vector3 targetLocation)
        {
            //Debug.Log($"Projectile Attack at {targetLocation.ToString()}");
        }
    }
}