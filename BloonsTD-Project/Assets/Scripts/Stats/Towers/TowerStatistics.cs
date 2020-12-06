using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    public class TowerStatistics : ScriptableObject
    {
        //TODO: change to collections

        public float AttackRange { get; private set; }
        public float AttackCooldownTime { get; private set; }
        public int NumPopped { get; private set; }

        public TowerUpgrade TowerUpgradePath1 { get; private set; }

        public TowerUpgrade TowerUpgradePath2 { get; private set; }

        public GameObject ProjectilePrefab { get; private set; }

        public void Set(TowerProperties properties)
        {
            TowerUpgradePath1 = properties.TowerUpgradePath1;
            TowerUpgradePath2 = properties.TowerUpgradePath2;
            AttackRange = properties.Range;
            AttackCooldownTime = properties.AttackCooldownTime;
            ProjectilePrefab = properties.ProjectilePrefab;
            NumPopped = 0;
        }
    }
}