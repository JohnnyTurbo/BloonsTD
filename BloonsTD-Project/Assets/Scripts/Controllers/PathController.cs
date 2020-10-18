using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _waypoints;
        [SerializeField] private float _pathWidth;
        
        public List<GameObject> Waypoints => _waypoints;

        public float PathWidth => _pathWidth;
    }
}