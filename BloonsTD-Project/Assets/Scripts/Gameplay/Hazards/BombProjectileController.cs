using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BombProjectileController : Hazard, IUpgradeRange
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _explosionRadius;
        
        private float _maxDistanceTraveled;
        private float MaxDistanceTraveled
        {
            get => _maxDistanceTraveled;
            set => _maxDistanceTraveled = Mathf.Max(float.Epsilon, value);
        }
        public void SetRange(float newRangeValue)
        {
            MaxDistanceTraveled = newRangeValue;
        }

        private Rigidbody2D _rigidbody;
        private Vector3 _startPosition;
        private float DistanceTraveled => Vector3.Distance(_startPosition, transform.position);

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            base.Start();
            _startPosition = transform.position;
            _rigidbody.AddForce(transform.up * _movementSpeed, ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (DistanceTraveled > _maxDistanceTraveled)
            {
                DestroyHazard();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsNotABloon()) return;
            Explode();
        }

        private void Explode()
        {
            var bloonCollidersInRange = new List<Collider2D>();
            var bloonFilter = new ContactFilter2D {layerMask = 1 << PhysicsLayers.Bloons, useLayerMask = true};
            Physics2D.OverlapCircle(transform.position, _explosionRadius, bloonFilter, bloonCollidersInRange);
            foreach (var bloonCollider in bloonCollidersInRange)
            {
                var bloonController = bloonCollider.GetComponent<BloonController>();
                if (!bloonController.BloonProperties.CanBePoppedByBombs) continue;
                bloonController.HitBloon(this);
            }
            DestroyHazard();
        }
    }
}