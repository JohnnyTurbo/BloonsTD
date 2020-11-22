using System;
using System.Collections;
using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class RoundController : MonoBehaviour
    {
        [SerializeField] private List<RoundSpawnStatistics> _rounds;

        private BloonSpawner _bloonSpawner;
        private bool _allBloonsSpawned;
        private int _bloonsLeft;
        public delegate void BloonSpawnedDelegate(BloonController bloonController);
        public event BloonSpawnedDelegate OnBloonSpawned;
        public delegate void RoundCompleteDelegate();
        public event RoundCompleteDelegate OnRoundComplete;
        private void OnEnable()
        {
            OnBloonSpawned += SetupBloonEvents;
        }

        private void OnDisable()
        {
            OnBloonSpawned -= SetupBloonEvents;
        }
        
        public BloonSpawner BloonSpawner
        {
            get => _bloonSpawner;
            set => _bloonSpawner = value;
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
                BloonController newBloonController = _bloonSpawner.SpawnBloonOfType(spawnGroup.BloonType);
                OnBloonSpawned?.Invoke(newBloonController);
                yield return timeBetweenSpawns;
            }
        }

        private void SetupBloonEvents(BloonController bloonController)
        {
            bloonController.OnBloonReachedEndOfPath += DecrementBloonsLeftCount;
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