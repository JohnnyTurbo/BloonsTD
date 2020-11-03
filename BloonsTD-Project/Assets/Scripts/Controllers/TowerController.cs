using System;
using JetBrains.Annotations;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerController : MonoBehaviour
    {
        private TowerProperties _towerProperties;
        private CircleCollider2D _collider;
        private LayerMask _towerPlacementMask;
        private bool _fullyOffPath;
        private bool _partiallyOnPath;
        private bool _outOfBounds;

        private Vector3[] _edgePoints;

        private float _colliderRadius;

        // TODO: Maybe place somewhere better??
        private int PathLayer => LayerMask.NameToLayer("BloonPath");
        private int OutOfBoundsLayer => LayerMask.NameToLayer("OutOfBounds");

        private bool FullyOnPath
        {
            get
            {
                if (!_partiallyOnPath)
                {
                    return false;
                }

                return CheckEdgePoints();
            }
        }

        private bool CheckEdgePoints()
        {
            foreach (var edgePoint in _edgePoints)
            {
                Vector3 pointToCheck = transform.position + edgePoint;

                Collider2D pathCollider = Physics2D.OverlapPoint(pointToCheck, 1 << PathLayer);

                if (pathCollider == null)
                {
                    return false;
                }
            }
            return true;
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
            SetupEdgePoints();
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
                Debug.Log($"X: {x}\nY: {y}");
                _edgePoints[i] = new Vector3(x, y);
                Debug.Log(_edgePoints[i]);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.layer.Equals(PathLayer))
            {
                _partiallyOnPath = true;
                _fullyOffPath = false;
            }

            if (other.gameObject.layer.Equals(OutOfBoundsLayer))
            {
                _outOfBounds = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer.Equals(PathLayer))
            {
                _partiallyOnPath = false;
                _fullyOffPath = true;
            }
            
            if (other.gameObject.layer.Equals(OutOfBoundsLayer))
            {
                _outOfBounds = false;
            }
        }

        public TowerProperties TowerProperties
        {
            get => _towerProperties;
            set => _towerProperties = value;
        }

        public bool ValidatePlacementPosition()
        {
            //Debug.Log("Validating Placement Position");
            if (_outOfBounds) { return false;}
            if (_towerProperties.CanBePlacedOffPath && _fullyOffPath)
            {
                return true;
            }

            if (_towerProperties.CanBePlacedOnPath && FullyOnPath)
            {
                return true;
            }
            
            return false;
        }
    }
}