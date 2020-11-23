using System;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] protected int _maxBloonHits;
        private int _bloonHitsRemaining;
        protected virtual void Start()
        {
            _bloonHitsRemaining = _maxBloonHits;
        }

        public virtual void HitBloon()
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