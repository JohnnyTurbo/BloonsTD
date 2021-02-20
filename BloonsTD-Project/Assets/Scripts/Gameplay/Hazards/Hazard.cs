using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class Hazard : MonoBehaviour
    {
        [SerializeField] protected int _maxBloonHits;
        [SerializeField] private bool _canPopFrozen;

        public bool CanPopFrozen => _canPopFrozen;

        private int _bloonHitsRemaining;
        protected List<BloonController> ImmuneBloons;
        protected int HitsThisStep;
        protected bool HitAnotherBloonThisStep => HitsThisStep > 1;
        protected virtual void Start()
        {
            _bloonHitsRemaining = _maxBloonHits;
            ImmuneBloons = new List<BloonController>();
        }

        private void OnEnable()
        {
            GameController.Instance.OnGameOver += DestroyHazard;
        }

        private void OnDisable()
        {
            GameController.Instance.OnGameOver -= DestroyHazard;
        }

        protected virtual void HitBloon(BloonController bloonToHit)
        {
            _bloonHitsRemaining--;
            if (_bloonHitsRemaining <= 0)
            {
                DestroyHazard();
            }
        }
        protected void DestroyHazard()
        {
            Destroy(gameObject);
        }
    }
}