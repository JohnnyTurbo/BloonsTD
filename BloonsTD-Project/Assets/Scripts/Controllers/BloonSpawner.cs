using System;
using UnityEngine;
using TMG.BloonsTD.Stats;

namespace TMG.BloonsTD.Controllers
{
    public class BloonSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _redBloonPrefab;
        [SerializeField] private GameObject _blueBloonPrefab;
        [SerializeField] private GameObject _greenBloonPrefab;
        [SerializeField] private GameObject _yellowBloonPrefab;
        [SerializeField] private GameObject _blackBloonPrefab;
        [SerializeField] private GameObject _whiteBloonPrefab;
        [SerializeField] private PathController _pathController;

        private Vector3 _spawnPosition;

        private void Start()
        {
            _spawnPosition = _pathController.Waypoints[0].transform.position;
        }

        public void SpawnBloon(BloonTypes bloonType)
        {
            GameObject newBloonGO;

            switch (bloonType)
            {
                case BloonTypes.Red:
                    newBloonGO = _redBloonPrefab;
                    break;
                case BloonTypes.Blue:
                    newBloonGO = _blueBloonPrefab;
                    break;
                case BloonTypes.Green:
                    newBloonGO = _greenBloonPrefab;
                    break;
                case BloonTypes.Yellow:
                    newBloonGO = _yellowBloonPrefab;
                    break;
                case BloonTypes.Black:
                    newBloonGO = _blackBloonPrefab;
                    break;
                case BloonTypes.White:
                    newBloonGO = _whiteBloonPrefab;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bloonType), bloonType, null);
            }
            
            Debug.Log($"Spawning {bloonType.ToString()}");
            Instantiate(newBloonGO, _spawnPosition, Quaternion.identity);
        }
    }
}