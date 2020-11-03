using System;
using TMG.BloonsTD.Controllers;
using UnityEngine;

namespace TMG.BloonsTD.Debug
{
    public class PathVisualization : MonoBehaviour
    {
        [SerializeField] private PathController _pathController;
        private void OnDrawGizmos()
        {
            if (_pathController == null ||
                (_pathController.Waypoints == null || _pathController.Waypoints.Count == 0)) return;

            var pathRadius = _pathController.PathRadius;
            
            for (int i = 0; i < _pathController.Waypoints.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_pathController.Waypoints[i].transform.position, pathRadius);
                
                if (i == _pathController.Waypoints.Count - 1) break;
                
                var pathStartPos = _pathController.Waypoints[i].transform.position;
                var pathEndPos = _pathController.Waypoints[i + 1].transform.position;

                Gizmos.DrawLine(pathStartPos, pathEndPos);
                
                var path = pathEndPos - pathStartPos;
                var pathCross = (Vector3.Cross(path, Vector3.forward)).normalized * pathRadius;
                
                var edge1Start = pathStartPos + pathCross;
                var edge1End = edge1Start + path;
                var edge2Start = pathStartPos - pathCross;
                var edge2End = edge2Start + path;
                
                Gizmos.DrawLine(edge1Start, edge1End);
                Gizmos.DrawLine(edge2Start, edge2End);
            }
        }
    }
}