using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class BloonController : MonoBehaviour
    {
        private BloonProperties _bloonProperties;
        private PathController _path;
        private int _targetWaypointIndex;
        private Vector3 _targetPosition;
        public BloonProperties BloonProperties
        {
            get => _bloonProperties;
            set => _bloonProperties = value;
        }

        public PathController Path
        {
            get => _path;
            set => _path = value;
        }

        public void InitializeTargetPosition(int targetWaypointIndex)
        {
            _targetWaypointIndex = targetWaypointIndex;
            _targetPosition = _path.Waypoints[_targetWaypointIndex].transform.position;
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
            if (_targetWaypointIndex >= _path.Waypoints.Count) {return;}
            _targetPosition = _path.Waypoints[_targetWaypointIndex].transform.position;
        }
    }
}