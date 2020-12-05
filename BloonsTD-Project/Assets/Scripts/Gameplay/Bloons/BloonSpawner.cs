using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    // TODO: This class DOES NOT support multiple bloon paths.
    public class BloonSpawner : MonoBehaviour
    {
        public static BloonSpawner Instance;

        public delegate void BloonSpawnedDelegate(BloonController bloonController);
        public event BloonSpawnedDelegate OnBloonSpawned;                          
        
        [SerializeField] private GameObject _bloonPrefab;
        [SerializeField] private PathController _pathController;
        
        private Vector3 _pathHeadPosition;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _pathHeadPosition = _pathController[0];
        }

        public void SpawnBloonOfType(BloonTypes bloonType)
        { 
            var bloonProperties = BloonPropertiesProcessor.GetBloonPropertiesFromBloonType(bloonType);
            SpawnBloon(bloonProperties, _pathHeadPosition, 0);
        }

        public void SpawnBloon(BloonProperties bloonProperties, Vector3 spawnPosition, int waypointIndex)
        {
            GameObject newBloon = Instantiate(_bloonPrefab, spawnPosition, Quaternion.identity);
            BloonController newBloonController = newBloon.GetComponent<BloonController>();

            newBloonController.Path = _pathController;
            newBloonController.InitializeBloon(bloonProperties, waypointIndex);
            OnBloonSpawned?.Invoke(newBloonController);
        }
    }
}