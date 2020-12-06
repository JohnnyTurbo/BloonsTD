using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicProjectileController : Hazard
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _maxDistanceTraveled;
        
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

        public override void HitBloon()
        {
            base.HitBloon();
        }
    }
}