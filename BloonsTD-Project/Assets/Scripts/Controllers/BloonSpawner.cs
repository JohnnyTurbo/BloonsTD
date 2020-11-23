using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Controllers
{
    public class BloonSpawner : MonoBehaviour
    {
        public static BloonSpawner Instance;

        public delegate void BloonSpawnedDelegate(BloonController bloonController);
        public event BloonSpawnedDelegate OnBloonSpawned;                          
        
        [SerializeField] private GameObject _bloonPrefab;
        [SerializeField] private PathController _pathController;
        
        private Vector3 _spawnPosition;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _spawnPosition = _pathController[0];
        }

        public void SpawnBloonOfType(BloonTypes bloonType)
        { 
            var bloonProperties = BloonPropertiesProcessor.GetBloonPropertiesFromBloonType(bloonType);
            SpawnBloon(bloonProperties, _spawnPosition, 0);
        }

        public void SpawnBloon(BloonProperties bloonProperties, Vector3 spawnPosition, int waypointIndex)
        {
            GameObject newBloon = Instantiate(_bloonPrefab, spawnPosition, Quaternion.identity);
            BloonController newBloonController = newBloon.GetComponent<BloonController>();

            newBloonController.BloonProperties = bloonProperties;

            newBloonController.Path = _pathController;
            newBloonController.InitializeTargetPosition(waypointIndex);
            OnBloonSpawned?.Invoke(newBloonController);
        }
    }
}