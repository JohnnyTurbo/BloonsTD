using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Controllers
{
    public class BloonSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _bloonPrefab;
        [SerializeField] private PathController _pathController;

        private Vector3 _spawnPosition;

        private void Start()
        {
            _spawnPosition = _pathController.Waypoints[0];
        }

        public BloonController SpawnBloon(BloonTypes bloonType)
        {
            
            GameObject newBloon = Instantiate(_bloonPrefab, _spawnPosition, Quaternion.identity);
            BloonController newBloonController = newBloon.GetComponent<BloonController>();

            newBloonController.BloonProperties = BloonPropertiesProcessor.GetBloonPropertiesFromBloonType(bloonType);

            newBloonController.Path = _pathController;
            newBloonController.InitializeTargetPosition(0);
            return newBloonController;
        }
    }
}