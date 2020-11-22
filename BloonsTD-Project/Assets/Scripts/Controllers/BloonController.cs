using System;
using TMG.BloonsTD.Helpers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    //[RequireComponent(typeof(Rigidbody2D))]
    public class BloonController : MonoBehaviour
    {
        public delegate void EndOfPathDelegate(int livesToLose);
        public delegate void BloonPoppedDelegate(BloonProperties bloonProperties);
        public event EndOfPathDelegate OnBloonReachedEndOfPath;
        public event BloonPoppedDelegate OnBloonPopped;
        private BloonProperties _bloonProperties;
        private PathController _bloonPath;
        private int _targetWaypointIndex;
        private int _hitsRemaining;
        private Vector3 _targetPosition;
        public int TargetWaypointIndex => _targetWaypointIndex;
        private Vector3 CurrentWaypoint => _bloonPath[_targetWaypointIndex];
        private Vector3 PreviousWaypoint =>
            _targetWaypointIndex == 0f ? Vector3.zero : _bloonPath[_targetWaypointIndex - 1];
        private float PathSegmentDistance =>
            _targetWaypointIndex == 0f ? 0 : Vector3.Distance(PreviousWaypoint, CurrentWaypoint);
        private float DistanceToNextWaypoint =>
            Vector3.Distance(transform.position, _bloonPath[_targetWaypointIndex]);
        public float PercentToNextWaypoint =>
            _targetWaypointIndex == 0f ? 0 : (PathSegmentDistance - DistanceToNextWaypoint) / PathSegmentDistance;

        public int RBE => _bloonProperties.RedBloonEquivalent;
        public BloonProperties BloonProperties
        {
            get => _bloonProperties;
            set => _bloonProperties = value;
        }

        public PathController Path
        {
            get => _bloonPath;
            set => _bloonPath = value;
        }

        private bool HasBloonsToSpawn => (_bloonProperties.BloonsToSpawnWhenPopped != null ||
                                          _bloonProperties.BloonsToSpawnWhenPopped.Count > 0);
        
        private void Start()
        {
            _hitsRemaining = BloonProperties.NumberOfHitsToPop;
        }

        public void InitializeTargetPosition(int targetWaypointIndex)
        {
            _targetWaypointIndex = targetWaypointIndex;
            _targetPosition = _bloonPath[_targetWaypointIndex];
            GetComponent<SpriteRenderer>().color = _bloonProperties.BloonColor;
        }

        private void Update()
        {
            float step = _bloonProperties.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            
            if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
            {
                SetNextTargetPosition();
            }
        }

        private void SetNextTargetPosition()
        {
            _targetWaypointIndex++;
            if (_targetWaypointIndex >= _bloonPath.WaypointCount)
            {
                OnBloonReachedEndOfPath?.Invoke(_bloonProperties.TotalBloonCount);
                Destroy(gameObject);
                return;
            }

            _targetPosition = _bloonPath[_targetWaypointIndex];
        }

        /*private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer.Equals(BloonsReferences.HazardsLayer))
            {
                var curHazard = other.gameObject.GetComponent<Hazard>();
                curHazard.HitBloon();
                _hitsRemaining--;
                if (_hitsRemaining <= 0)
                {
                    PopBloon();
                }
            }
        }*/

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("OTE");
            if (other.gameObject.layer.Equals(BloonsReferences.HazardsLayer))
            {
                var curHazard = other.gameObject.GetComponent<Hazard>();
                curHazard.HitBloon();
                _hitsRemaining--;
                if (_hitsRemaining <= 0)
                {
                    PopBloon();
                }
            }
        }

        private void PopBloon()
        {
            OnBloonPopped?.Invoke(_bloonProperties);
            if (HasBloonsToSpawn)
            {
                SpawnChildBloons();
            }
            Destroy(gameObject);
        }

        private void SpawnChildBloons()
        {
            foreach (var bloonToSpawn in _bloonProperties.BloonsToSpawnWhenPopped)
            {
                BloonSpawner.Instance.SpawnBloon(bloonToSpawn);
            }
        }
    }
}