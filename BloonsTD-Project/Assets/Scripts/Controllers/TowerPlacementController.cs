using System;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.Helpers;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerPlacementController : MonoBehaviour
    {
        private TowerProperties _towerProperties;
        private CircleCollider2D _collider;
        private LayerMask _towerPlacementMask;
        private bool _fullyOffPath;
        private bool _partiallyOnPath;
        private bool _outOfBounds;
        private bool _overlappingTower;
        private bool _lastPlacementStatus;
        private Vector3[] _edgePoints;
        private float _colliderRadius;
        //private TowerSpawnManager _towerSpawnManager;

        public delegate void ChangeRangeIndicator(bool isValidPosition);

        public delegate void HideRangeIndicator();

        public event ChangeRangeIndicator OnChangeRangeIndicator;
        public event HideRangeIndicator OnHideRangeIndicator;
        
        public bool IsValidPlacementPosition
        {
            get
            {
                if (_outOfBounds) { return false; }
                if (_overlappingTower) { return false; }
                if (_towerProperties.CanBePlacedOffPath && _fullyOffPath) { return true; }
                if (_towerProperties.CanBePlacedOnPath && IsFullyOnPath) { return true; }
                return false;
            }
        }

        private bool IsFullyOnPath => _partiallyOnPath && CheckIfFullyOnPath();

        public TowerProperties TowerProperties
        {
            get => _towerProperties;
            set => _towerProperties = value;
        }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            if (_collider == null)
            {
                _collider = gameObject.AddComponent<CircleCollider2D>();
            }
        }

        private void Start()
        {
            _colliderRadius = _towerProperties.ColliderRadius;
            _collider.radius = _colliderRadius;
            _fullyOffPath = true;
            _outOfBounds = true;
            _lastPlacementStatus = true;
            SetupEdgePoints();
        }

        private void OnEnable()
        {
            TowerSpawnManager.Instance.OnTowerPlaced += OnTowerPlaced;
        }

        private void SetupEdgePoints()
        {
            _edgePoints = new Vector3[9];
            _edgePoints[0] = Vector3.zero;
            for (int i = 1; i < _edgePoints.Length; i++)
            {
                var degree = (i - 1) * 45f;
                var x = _colliderRadius * Mathf.Cos(degree * Mathf.Deg2Rad);
                var y = _colliderRadius * Mathf.Sin(degree * Mathf.Deg2Rad);
                _edgePoints[i] = new Vector3(x, y);
            }
        }

        private bool CheckIfFullyOnPath()
        {
            foreach (var edgePoint in _edgePoints)
            {
                Vector3 pointToCheck = transform.position + edgePoint;

                Collider2D pathCollider = Physics2D.OverlapPoint(pointToCheck, 1 << BloonsReferences.PathLayer);

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
            
            if (_lastPlacementStatus != isValidPlacementPosition)
            {
                OnChangeRangeIndicator?.Invoke(isValidPlacementPosition);
                _lastPlacementStatus = isValidPlacementPosition;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer.Equals(BloonsReferences.PathLayer))
            {
                _partiallyOnPath = true;
                _fullyOffPath = false;
            }

            if (other.gameObject.layer.Equals(BloonsReferences.OutOfBoundsLayer))
            {
                _outOfBounds = true;
            }

            if (other.gameObject.layer.Equals(BloonsReferences.TowerLayer))
            {
                _overlappingTower = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer.Equals(BloonsReferences.PathLayer))
            {
                _partiallyOnPath = false;
                _fullyOffPath = true;
            }
            
            if (other.gameObject.layer.Equals(BloonsReferences.OutOfBoundsLayer))
            {
                _outOfBounds = false;
            }
            
            if (other.gameObject.layer.Equals(BloonsReferences.TowerLayer))
            {
                _overlappingTower = false;
            }
        }
        
        private void OnTowerPlaced(TowerController towerController)
        {
            OnHideRangeIndicator?.Invoke();
            enabled = false;
        }
    }
}