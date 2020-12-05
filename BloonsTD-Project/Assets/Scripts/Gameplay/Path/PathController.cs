using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _waypoints;
        [SerializeField] private float _pathWidth;
        [SerializeField] private EdgeCollider2D _edgeCollider;
        private float _pathDistance;
        public int WaypointCount => _waypoints?.Count ?? 0;
        public Vector3 this[int i] => _waypoints[i].transform.position;
        public float PathWidth => _pathWidth;
        public float PathRadius => _pathWidth / 2;
        public float PathDistance => _pathDistance;
        private void Start()
        {
            SetPathCollider();
        }

        private void SetPathCollider()
        {
            var wpPositions = new List<Vector2>();
            _pathDistance = 0f;
            for (var i = 0; i < _waypoints.Count; i++)
            {
                var waypoint = _waypoints[i];
                wpPositions.Add(waypoint.transform.position);
                if (i != 0)
                {
                    _pathDistance +=
                        Vector2.Distance(waypoint.transform.position, _waypoints[i - 1].transform.position);
                }
            }

            var wpArray = wpPositions.ToArray();
            _edgeCollider.points = wpArray;
            _edgeCollider.edgeRadius = PathRadius;
        }
    }
}