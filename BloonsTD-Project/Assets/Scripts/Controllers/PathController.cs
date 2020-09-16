using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _waypoints;

        public List<GameObject> Waypoints => _waypoints;
    }
}