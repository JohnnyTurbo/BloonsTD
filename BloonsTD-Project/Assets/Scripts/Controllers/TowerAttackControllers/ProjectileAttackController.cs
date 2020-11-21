using System;
using System.Collections.Generic;
using TMG.BloonsTD.Helpers;
using UnityEngine;

namespace TMG.BloonsTD.Controllers.TowerAttackControllers
{
    public class ProjectileAttackController : TowerAttackController
    {
        private GameObject _projectile;

        public GameObject Projectile
        {
            get => _projectile;
            set => _projectile = value;
        }

        private void Start()
        {
            _projectile = _towerController.TowerProperties.ProjectilePrefab;
        }

        protected override void Attack(Vector3 targetLocation)
        {
            //Debug.DrawLine(transform.position, targetLocation, Color.red, 1f);
            var towerToTarget = targetLocation - transform.position;
            var angle = Vector3.SignedAngle(Vector3.up, towerToTarget, Vector3.forward);
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
            Instantiate(_projectile, transform.position, rotation);
        }
    }
}