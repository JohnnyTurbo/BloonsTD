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
            _collider.radius = _towerProperties.ColliderRadius;
            _fullyOffPath = true;
            _outOfBounds = true;
            float colliderRadius = _towerProperties.ColliderRadius;
            _edgePoints = new[]
            {
                new Vector3(0, colliderRadius), 
                new Vector3(colliderRadius, 0), 
                new Vector3(0, -colliderRadius),
                new Vector3(-colliderRadius, 0)
            };
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
            Debug.Log("Validating Placement Position");
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