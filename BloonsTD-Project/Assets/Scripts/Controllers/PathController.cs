using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _waypoints;
        [SerializeField] private float _pathWidth;
        [SerializeField] private EdgeCollider2D _edgeCollider;
        public int WaypointCount => _waypoints?.Count ?? 0;
        public Vector3 this[int i] => _waypoints[i].transform.position;
        public float PathWidth => _pathWidth;
        public float PathRadius => _pathWidth / 2;
        private void Start()
        {
            SetPathCollider();
        }

        private void SetPathCollider()
        {
            var wpPositions = new List<Vector2>();
            foreach (var waypoint in _waypoints)
            {
                wpPositions.Add(waypoint.transform.position);
            }

            var wpArray = wpPositions.ToArray();
            _edgeCollider.points = wpArray;
            _edgeCollider.edgeRadius = PathRadius;
        }
    }
}