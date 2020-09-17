using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

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

        public void SpawnBloon(BloonTypes bloonType)
        {
            Debug.Log($"Spawning {bloonType} bloon");
            GameObject newBloon = Instantiate(_bloonPrefab, _spawnPosition, Quaternion.identity);
            BloonController newBloonController = newBloon.GetComponent<BloonController>();

            switch (bloonType)
            {
                case BloonTypes.Red:
                    newBloonController.BloonProperties = _redBloonProperties;
                    break;
                case BloonTypes.Blue:
                    newBloonController.BloonProperties = _blueBloonProperties;
                    break;
                case BloonTypes.Green:
                    newBloonController.BloonProperties = _greenBloonProperties;
                    break;
                case BloonTypes.Yellow:
                    newBloonController.BloonProperties = _yellowBloonProperties;
                    break;
                case BloonTypes.Black:
                    newBloonController.BloonProperties = _blackBloonProperties;
                    break;
                case BloonTypes.White:
                    newBloonController.BloonProperties = _whiteBloonProperties;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bloonType), bloonType, null);
            }
            
            newBloonController.Path = _pathController;
            newBloonController.InitializeTargetPosition(0);
        }
    }
}