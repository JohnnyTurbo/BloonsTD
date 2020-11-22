using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Controllers
{
    public class BloonSpawner : MonoBehaviour
    {
        public static BloonSpawner Instance;
        
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

        public BloonController SpawnBloonOfType(BloonTypes bloonType)
        { 
            var bloonProperties = BloonPropertiesProcessor.GetBloonPropertiesFromBloonType(bloonType);
            return SpawnBloon(bloonProperties);
        }

        public BloonController SpawnBloon(BloonProperties bloonProperties)
        {
            GameObject newBloon = Instantiate(_bloonPrefab, _spawnPosition, Quaternion.identity);
            BloonController newBloonController = newBloon.GetComponent<BloonController>();

            newBloonController.BloonProperties = bloonProperties;

            newBloonController.Path = _pathController;
            newBloonController.InitializeTargetPosition(0);
            return newBloonController;
        }
    }
}