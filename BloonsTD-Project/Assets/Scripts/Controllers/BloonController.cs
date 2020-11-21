using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class BloonController : MonoBehaviour
    {
        public delegate void EndOfPathDelegate(int livesToLose);

        public event EndOfPathDelegate OnBloonReachedEndOfPath;
        private BloonProperties _bloonProperties;
        private PathController _bloonPath;
        private int _targetWaypointIndex;
        private Vector3 _targetPosition;

        public int TargetWaypointIndex => _targetWaypointIndex;
        private Vector3 CurrentWaypoint => _bloonPath[_targetWaypointIndex];
        private Vector3 PreviousWaypoint =>
            _targetWaypointIndex == 0f ? Vector3.zero : _bloonPath[_targetWaypointIndex - 1];
        private float PathSegmentDistance =>
            _targetWaypointIndex == 0f ? 0 : Vector3.Distance(PreviousWaypoint, CurrentWaypoint);
        private float DistanceToNextWaypoint =>
            Vector3.Distance(transform.position, _bloonPath[_targetWaypointIndex]);
        public float PercentToNextWaypoint =>
            _targetWaypointIndex == 0f ? 0 : (PathSegmentDistance - DistanceToNextWaypoint) / PathSegmentDistance;

        public int RBE => _bloonProperties.RedBloonEquivalent;
        public BloonProperties BloonProperties
        {
            get => _bloonProperties;
            set => _bloonProperties = value;
        }

        public PathController Path
        {
            get => _bloonPath;
            set => _bloonPath = value;
        }

        public void InitializeTargetPosition(int targetWaypointIndex)
        {
            _targetWaypointIndex = targetWaypointIndex;
            _targetPosition = _bloonPath[_targetWaypointIndex];
            GetComponent<SpriteRenderer>().color = _bloonProperties.BloonColor;
        }

        private void Update()
        {
            float step = _bloonProperties.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            
            if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
            {
                SetNextTargetPosition();
            }
        }

        private void SetNextTargetPosition()
        {
            _targetWaypointIndex++;
            if (_targetWaypointIndex >= _bloonPath.WaypointCount)
            {
                OnBloonReachedEndOfPath?.Invoke(_bloonProperties.TotalBloonCount);
                Destroy(gameObject);
                return;
            }

            _targetPosition = _bloonPath[_targetWaypointIndex];
        }
    }
}