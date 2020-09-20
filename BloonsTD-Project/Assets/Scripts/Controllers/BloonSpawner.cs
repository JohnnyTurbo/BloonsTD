using System;
using UnityEngine;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.Helpers;

namespace TMG.BloonsTD.Controllers
{
    public class BloonSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _bloonPrefab;
        [SerializeField] private BloonProperties _bloonProperties;
        
        [SerializeField] private BloonProperties _redBloonProperties;
        [SerializeField] private BloonProperties _blueBloonProperties;
        [SerializeField] private BloonProperties _greenBloonProperties;
        [SerializeField] private BloonProperties _yellowBloonProperties;
        [SerializeField] private BloonProperties _blackBloonProperties;
        [SerializeField] private BloonProperties _whiteBloonProperties;
        
        [SerializeField] private PathController _pathController;

        private Vector3 _spawnPosition;

        private void Start()
        {
            _spawnPosition = _pathController.Waypoints[0].transform.position;
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