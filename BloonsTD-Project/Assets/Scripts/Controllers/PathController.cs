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
        
        public List<Vector3> Waypoints => _waypoints.Select(wp => wp.transform.position) as List<Vector3>;

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