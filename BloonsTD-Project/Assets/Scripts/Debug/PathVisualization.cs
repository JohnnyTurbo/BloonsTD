using System;
using System.Collections.Generic;
using System.Linq;
using TMG.BloonsTD.Gameplay;
using UnityEngine;

namespace TMG.BloonsTD.Debug
{
    public class PathVisualization : MonoBehaviour
    {
        [SerializeField] private PathController _bloonPath;
        private void OnDrawGizmos()
        {
            if (_bloonPath == null || _bloonPath.WaypointCount == 0) return;

            var pathRadius = _bloonPath.PathRadius;
            
            for (int i = 0; i < _bloonPath.WaypointCount; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_bloonPath[i], pathRadius);
                
                if (i == _bloonPath.WaypointCount - 1) break;

                var pathStartPos = _bloonPath[i];
                var pathEndPos = _bloonPath[i + 1];

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