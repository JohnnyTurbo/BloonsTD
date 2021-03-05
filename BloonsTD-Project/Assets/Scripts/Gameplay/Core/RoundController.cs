using System.Collections;
using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public class RoundController : MonoBehaviour
    {
        public delegate void RoundEventDelegate(RoundProperties round);
        public event RoundEventDelegate OnRoundComplete;
        public event RoundEventDelegate OnQueueNextRound;
        
        [SerializeField] private List<RoundProperties> _rounds;
        [SerializeField] private GameController _gameController;
        
        private BloonSpawner _bloonSpawner;
        private int _bloonsLeft;
        private RoundProperties _curRound;
        private GameStatistics _curGameStatistics;
        private int CurRoundIndex => _curGameStatistics.Rounds - 1;
        private void OnEnable()
        {
            _bloonSpawner = BloonSpawner.Instance;
            _bloonSpawner.OnBloonSpawned += SetupBloonEvents;
            _gameController.OnGameBegin += QueueNextRound;
        }

        private void OnDisable()
        {
            _bloonSpawner.OnBloonSpawned -= SetupBloonEvents;
            _gameController.OnGameBegin -= QueueNextRound;
        }

        public void Initialize(GameStatistics curGameStatistics)
        {
            _curGameStatistics = curGameStatistics;
            
            for (var i = 0; i < _rounds.Count; i++)
            {
                _rounds[i].RoundNumber = i + 1;
            }
        }

        private void QueueNextRound()
        {
            _curGameStatistics.Rounds++;
            if (_curGameStatistics.Rounds > _rounds.Count)
            {
                _gameController.BeginVictory();
                return;
            }
            if (_curGameStatistics.Rounds != 1)
            {
                OnRoundComplete?.Invoke(_curRound);
            }
            
            _curRound = _rounds[CurRoundIndex];
            _bloonsLeft = _curRound.TotalBloonCount;
            OnQueueNextRound?.Invoke(_curRound);
        }
        
        public void StartNextRound()
        {
            StartCoroutine(SpawnBloonsInRound(_curRound));
        }

        private IEnumerator SpawnBloonsInRound(RoundProperties roundProperties)
        {
            foreach (var spawnGroup in roundProperties.SpawnGroups)
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
                QueueNextRound();
            }
        }
    }
}