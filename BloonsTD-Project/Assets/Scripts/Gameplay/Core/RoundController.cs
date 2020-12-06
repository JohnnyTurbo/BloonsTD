using System.Collections;
using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class RoundController : MonoBehaviour
    {
        public delegate void RoundCompleteDelegate();
        public event RoundCompleteDelegate OnRoundComplete;
        
        [SerializeField] private List<RoundSpawnStatistics> _rounds;
        [SerializeField] private GameController _gameController;
        
        private BloonSpawner _bloonSpawner;
        private int _bloonsLeft;

        private void OnEnable()
        {
            _bloonSpawner = BloonSpawner.Instance;
            _bloonSpawner.OnBloonSpawned += SetupBloonEvents;
        }

        private void OnDisable()
        {
            _bloonSpawner.OnBloonSpawned -= SetupBloonEvents;
        }

        public void StartRound(int roundNumber)
        {
            int roundIndex = roundNumber - 1;
            var currentRound = _rounds[roundIndex];
            _bloonsLeft = currentRound.TotalBloonCount;
            StartCoroutine(SpawnBloonsInRound(currentRound));
        }

        private IEnumerator SpawnBloonsInRound(RoundSpawnStatistics roundSpawnStatistics)
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
            if (_gameController.GameOver) return;
            _bloonsLeft -= numberToDecrement;
            if (_bloonsLeft <= 0)
            {
                OnRoundComplete?.Invoke();
            }
        }
    }
}