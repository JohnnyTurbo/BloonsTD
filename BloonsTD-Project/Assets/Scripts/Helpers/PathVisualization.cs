using System;
using TMG.BloonsTD.Controllers;
using UnityEngine;

namespace Helpers
{
    public class PathVisualization : MonoBehaviour
    {
        [SerializeField] private PathController _pathController;

        private void OnDrawGizmos()
        {
            if (_pathController == null ||
                (_pathController.Waypoints == null || _pathController.Waypoints.Count == 0)) return;
            
            
            
            for (int i = 0; i < _pathController.Waypoints.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_pathController.Waypoints[i].transform.position, _pathController.PathWidth);
                if (i == _pathController.Waypoints.Count - 1) break;
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_pathController.Waypoints[i].transform.position,
                    _pathController.Waypoints[i + 1].transform.position);
            }
        }
    }
}