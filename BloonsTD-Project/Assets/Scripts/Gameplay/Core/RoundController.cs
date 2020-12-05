using System.Collections;
using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class RoundController : MonoBehaviour
    {
        [SerializeField] private List<RoundSpawnStatistics> _rounds;

        private BloonSpawner _bloonSpawner;
        private bool _allBloonsSpawned;
        private int _bloonsLeft;
        //public delegate void BloonSpawnedDelegate(BloonController bloonController);
        //public event BloonSpawnedDelegate OnBloonSpawned;
        public delegate void RoundCompleteDelegate();
        public event RoundCompleteDelegate OnRoundComplete;

        private void Awake()
        {
            _bloonSpawner = BloonSpawner.Instance;
        }

        private void OnEnable()
        {
            _bloonSpawner.OnBloonSpawned += SetupBloonEvents;
        }

        private void OnDisable()
        {
            _bloonSpawner.OnBloonSpawned -= SetupBloonEvents;
        }

        public void StartRound(int roundNumber)
        {
            int roundIndex = roundNumber - 1;
            //Debug.Log($"Round RBE: {_rounds[roundIndex].RedBloonEquivalent}\nNumBloons: {_rounds[roundIndex].TotalBloonCount}");
            _bloonsLeft = _rounds[roundIndex].TotalBloonCount;
            StartCoroutine(SpawnBloonGroups(_rounds[roundIndex]));
        }

        private IEnumerator SpawnBloonGroups(RoundSpawnStatistics roundSpawnStatistics)
        {
            foreach (var spawnGroup in roundSpawnStatistics.SpawnGroups)
            {
                yield return new WaitForSeconds(spawnGroup.InitialSpawnDelay);
                StartCoroutine(SpawnBloonsInGroup(spawnGroup));
            }
        }

        private IEnumerator SpawnBloonsInGroup(SpawnGroup spawnGroup)
        {
            WaitForSeconds timeBetweenSpawns = new WaitForSeconds(spawnGroup.TimeBetweenBloons);
            for (int i = 0; i < spawnGroup.NumberInGroup; i++)
            {
                _bloonSpawner.SpawnBloonOfType(spawnGroup.BloonType);
                yield return timeBetweenSpawns;
            }
        }

        private void SetupBloonEvents(BloonController bloonController)
        {
            bloonController.OnBloonReachedEndOfPath += BloonEndOfPath;
            bloonController.OnBloonPopped += BloonPopped;
        }

        private void BloonPopped(BloonProperties bloonProperties)
        {
            DecrementBloonsLeftCount(1);
        }

        private void BloonEndOfPath(BloonProperties bloonProperties)
        {
            DecrementBloonsLeftCount(bloonProperties.TotalBloonCount);
        }
        
        private void DecrementBloonsLeftCount(int numberToDecrement)
        {
            _bloonsLeft -= numberToDecrement;
            if (_bloonsLeft <= 0)
            {
                OnRoundComplete?.Invoke();
            }
        }
    }
}