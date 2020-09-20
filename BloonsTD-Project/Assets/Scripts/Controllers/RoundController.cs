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
        //private int 
        public delegate void BloonSpawnedDelegate(BloonController bloonController);

        public event BloonSpawnedDelegate OnBloonSpawned;
        public BloonSpawner BloonSpawner
        {
            get => _bloonSpawner;
            set => _bloonSpawner = value;
        }

        public void StartRound(int roundNumber)
        {
            int roundIndex = roundNumber - 1;
            //Debug.Log($"Starting Round: {_rounds[roundIndex].RoundTime}");
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
                BloonController newBloonController = _bloonSpawner.SpawnBloon(spawnGroup.BloonType);
                OnBloonSpawned?.Invoke(newBloonController);
                yield return timeBetweenSpawns;
            }
        }
    }
}