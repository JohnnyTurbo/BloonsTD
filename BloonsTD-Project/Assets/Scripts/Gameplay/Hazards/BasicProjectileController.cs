using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicProjectileController : Hazard, IUpgradeRange
    {
        [SerializeField] private float _movementSpeed;
        
        private float _maxDistanceTraveled;
        private float MaxDistanceTraveled
        {
            get => _maxDistanceTraveled;
            set => _maxDistanceTraveled = Mathf.Max(float.Epsilon, value);
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

        private void FixedUpdate()
        {
            HitsThisStep = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsNotABloon()) return;
            var curBloon = other.GetComponent<BloonController>();
            if (ImmuneBloons.Contains(curBloon)) return;
            HitsThisStep++;
            if (HitAnotherBloonThisStep) return;
            HitBloon(curBloon);
        }

        protected override void HitBloon(BloonController bloonToHit)
        {
            if (bloonToHit.HitBloon(this, out var newImmuneBloons))
            {
                ImmuneBloons.AddRange(newImmuneBloons);
                base.HitBloon(bloonToHit);
            }
            else
            {
                DestroyHazard();   
            }
        }

        public void SetRange(float newRangeValue)
        {
            MaxDistanceTraveled = newRangeValue;
        }
    }
}