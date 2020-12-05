using TMG.BloonsTD.Helpers;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BloonController : MonoBehaviour
    {
        public delegate void BloonEndOfLife(BloonProperties bloonProperties);
        public event BloonEndOfLife OnBloonReachedEndOfPath;
        public event BloonEndOfLife OnBloonPopped;
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
                OnBloonReachedEndOfPath?.Invoke(_bloonProperties);
                Destroy(gameObject);
                return;
            }

            _targetPosition = _bloonPath[_targetWaypointIndex];
        }

        public void HitBloon()
        {
            _hitsRemaining--;
            if (_hitsRemaining <= 0)
            {
                PopBloon();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
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
                BloonSpawner.Instance.SpawnBloon(bloonToSpawn, transform.position, _targetWaypointIndex);
            }
        }

        public static BloonController CompareGreaterPathProgress(BloonController bloon1, BloonController bloon2)
        {
            if (bloon1.TargetWaypointIndex > bloon2.TargetWaypointIndex)
                return bloon1;
            if (bloon1.TargetWaypointIndex < bloon2.TargetWaypointIndex)
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
            if (bloon1.TargetWaypointIndex < bloon2.TargetWaypointIndex)
                return bloon1;
            if (bloon1.TargetWaypointIndex > bloon2.TargetWaypointIndex)
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