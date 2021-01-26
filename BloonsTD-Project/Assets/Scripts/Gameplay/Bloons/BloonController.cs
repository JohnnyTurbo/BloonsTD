using System;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BloonController : MonoBehaviour
    {
        public delegate void BloonEndOfLife(BloonProperties bloonProperties);
        public event BloonEndOfLife OnBloonReachedEndOfPath;
        public event BloonEndOfLife OnBloonPopped;
        
        private BloonProperties _bloonProperties;
        private PathController _bloonPath;
        private SpriteRenderer _renderer;
        private int _targetWaypointIndex;
        private int _hitsRemaining;
        private Vector3 _targetWaypointPosition;
        //private Vector3 CurrentWaypoint => _bloonPath[_targetWaypointIndex];
        private Vector3 PreviousWaypointPosition =>
            _targetWaypointIndex == 0f ? Vector3.zero : _bloonPath[_targetWaypointIndex - 1];
        private float PathSegmentDistance =>
            _targetWaypointIndex == 0f ? 0 : Vector3.Distance(PreviousWaypointPosition, _targetWaypointPosition);
        private float DistanceToNextWaypoint => Vector3.Distance(transform.position, _targetWaypointPosition);

        private float PercentToNextWaypoint =>
            _targetWaypointIndex == 0f ? 0 : (PathSegmentDistance - DistanceToNextWaypoint) / PathSegmentDistance;


        private bool BloonReachedEndOfPath => _targetWaypointIndex >= _bloonPath.WaypointCount;
        private int RBE => _bloonProperties.RedBloonEquivalent;

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

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            float step = _bloonProperties.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetWaypointPosition, step);
            
            if (Vector3.Distance(transform.position, _targetWaypointPosition) <= 0.1f)
            {
                SetNextTargetPosition();
            }
        }

        public void InitializeBloon(BloonProperties bloonProperties, int targetWaypointIndex)
        {
            _bloonProperties = bloonProperties;
            _hitsRemaining = _bloonProperties.NumberOfHitsToPop;
            _renderer.color = _bloonProperties.BloonColor;
            _targetWaypointIndex = targetWaypointIndex;
            SetTargetWaypointPosition();
        }

        private void SetNextTargetPosition()
        {
            _targetWaypointIndex++;
            if (BloonReachedEndOfPath)
            {
                OnBloonReachedEndOfPath?.Invoke(_bloonProperties);
                Destroy(gameObject);
                return;
            }
            SetTargetWaypointPosition();
        }

        private void SetTargetWaypointPosition()
        {
            _targetWaypointPosition = _bloonPath[_targetWaypointIndex];
        }

        public BloonController[] HitBloon()
        {
            _hitsRemaining--;
            return _hitsRemaining <= 0 ? PopBloon() : new BloonController[1] {this};
        }

        private BloonController[] PopBloon()
        {
            OnBloonPopped?.Invoke(_bloonProperties);
            if (HasBloonsToSpawn)
            {
                Destroy(gameObject);
                return SpawnChildBloons();
            }
            else
            {
                Destroy(gameObject);
                return null;
            }
        }

        private BloonController[] SpawnChildBloons()
        {
            var childBloons = new BloonController[_bloonProperties.BloonsToSpawnWhenPopped.Count];
            for (var i = 0; i < _bloonProperties.BloonsToSpawnWhenPopped.Count; i++)
            {
                var bloonToSpawn = _bloonProperties.BloonsToSpawnWhenPopped[i];
                var newBloon = BloonSpawner.Instance.SpawnBloon(bloonToSpawn, transform.position, _targetWaypointIndex);
                childBloons[i] = newBloon;
            }
            return childBloons;
        }

        // TODO: Path progress comparisons DO NOT support multiple bloon paths.
        
        public static BloonController CompareGreaterPathProgress(BloonController bloon1, BloonController bloon2)
        {
            if (bloon1._targetWaypointIndex > bloon2._targetWaypointIndex)
                return bloon1;
            if (bloon1._targetWaypointIndex < bloon2._targetWaypointIndex)
                return bloon2;
            if (bloon1.PercentToNextWaypoint > bloon2.PercentToNextWaypoint)
                return bloon1;
            if (bloon1.PercentToNextWaypoint < bloon2.PercentToNextWaypoint)
                return bloon2;
            else
                return bloon1;
        }
        public static BloonController CompareLeastPathProgress(BloonController bloon1, BloonController bloon2)
        {
            if (bloon1._targetWaypointIndex < bloon2._targetWaypointIndex)
                return bloon1;
            if (bloon1._targetWaypointIndex > bloon2._targetWaypointIndex)
                return bloon2;
            if (bloon1.PercentToNextWaypoint < bloon2.PercentToNextWaypoint)
                return bloon1;
            if (bloon1.PercentToNextWaypoint > bloon2.PercentToNextWaypoint)
                return bloon2;
            else
                return bloon1;
        }

        public static BloonController CompareStrongest(BloonController bloon1, BloonController bloon2)
        {
            if (bloon1.RBE > bloon2.RBE)
                return bloon1;
            if (bloon1.RBE < bloon2.RBE)
                return bloon2;
            return CompareGreaterPathProgress(bloon1, bloon2);
        }
        
        public static BloonController CompareWeakest(BloonController bloon1, BloonController bloon2)
        {
            if (bloon1.RBE < bloon2.RBE)
                return bloon1;
            if (bloon1.RBE > bloon2.RBE)
                return bloon2;
            return CompareGreaterPathProgress(bloon1, bloon2);
        }
    }
}