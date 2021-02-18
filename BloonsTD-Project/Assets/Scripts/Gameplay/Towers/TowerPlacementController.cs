using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TowerPlacementController : MonoBehaviour
    {
        private CircleCollider2D _collider;
        private LayerMask _towerPlacementMask;
        private bool _fullyOffPath;
        private bool _partiallyOnPath;
        private bool _outOfBounds;
        private bool _overlappingTower;
        private bool _lastPlacementStatus;
        private Vector3[] _placementValidationPoints;
        private float _colliderRadius;

        [SerializeField] private int _externalValidationPointsCount;
        //private TowerSpawnManager _towerSpawnManager;

        public delegate void ChangeRangeIndicator(bool isValidPosition);

        public delegate void HideRangeIndicator();

        public event ChangeRangeIndicator OnChangeRangeIndicator;
        public event HideRangeIndicator OnHideRangeIndicator;
        public TowerProperties TowerProperties { get; set; }

        public bool IsValidPlacementPosition
        {
            get
            {
                if (_outOfBounds) { return false; }
                if (_overlappingTower) { return false; }
                if (TowerProperties.CanBePlacedOffPath && _fullyOffPath) { return true; }
                if (TowerProperties.CanBePlacedOnPath && IsFullyOnPath) { return true; }
                return false;
            }
        }

        private bool IsFullyOnPath => _partiallyOnPath && CheckIfFullyOnPath();
        
        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            _colliderRadius = TowerProperties.ColliderRadius;
            _collider.radius = _colliderRadius;
            _fullyOffPath = true;
            _outOfBounds = true;
            _lastPlacementStatus = true;
            InitializePlacementValidationPoints();
        }

        private void OnEnable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced += OnTowerPlaced;
        }
        
        private void OnDisable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced -= OnTowerPlaced;
        }

        private void InitializePlacementValidationPoints()
        {
            _placementValidationPoints = new Vector3[_externalValidationPointsCount + 1];
            _placementValidationPoints[0] = Vector3.zero;
            var degree = 360 / _externalValidationPointsCount;
            for (int i = 1; i < _placementValidationPoints.Length; i++)
            {
                var pointDegree = (i - 1) * degree;
                var x = _colliderRadius * Mathf.Cos(pointDegree * Mathf.Deg2Rad);
                var y = _colliderRadius * Mathf.Sin(pointDegree * Mathf.Deg2Rad);
                _placementValidationPoints[i] = new Vector3(x, y);
            }
        }

        private bool CheckIfFullyOnPath()
        {
            foreach (var validationPoint in _placementValidationPoints)
            {
                Vector3 worldValidationPosition = transform.position + validationPoint;

                Collider2D pathCollider = Physics2D.OverlapPoint(worldValidationPosition, 1 << PhysicsLayers.Path);

                if (pathCollider == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void Update()
        {
            var isValidPlacementPosition = IsValidPlacementPosition;

            bool placementStatusChanged = _lastPlacementStatus != isValidPlacementPosition;
            if (!placementStatusChanged) return;
            OnChangeRangeIndicator?.Invoke(isValidPlacementPosition);
            _lastPlacementStatus = isValidPlacementPosition;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.IsOnLayer(PhysicsLayers.Path))
            {
                _partiallyOnPath = true;
                _fullyOffPath = false;
            }

            if (otherCollider.IsOnLayer(PhysicsLayers.OutOfBounds))
            {
                _outOfBounds = true;
            }

            if (otherCollider.IsOnLayer(PhysicsLayers.Towers))
            {
                _overlappingTower = true;
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (otherCollider.IsOnLayer(PhysicsLayers.Path))
            {
                _partiallyOnPath = false;
                _fullyOffPath = true;
            }
            
            if (otherCollider.IsOnLayer(PhysicsLayers.OutOfBounds))
            {
                _outOfBounds = false;
            }
            
            if (otherCollider.IsOnLayer(PhysicsLayers.Towers))
            {
                _overlappingTower = false;
            }
        }
        
        private void OnTowerPlaced(TowerController towerController)
        {
            _collider.isTrigger = true;
            OnHideRangeIndicator?.Invoke();
            enabled = false;
        }
    }
}