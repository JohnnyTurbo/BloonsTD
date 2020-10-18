using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerController : MonoBehaviour
    {
        private TowerProperties _towerProperties;

        public TowerProperties TowerProperties
        {
            get => _towerProperties;
            set => _towerProperties = value;
        }
    }
}