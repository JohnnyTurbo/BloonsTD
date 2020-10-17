using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public class TowerSpawnController : MonoBehaviour
    {
        public void SpawnTower(BloonProperties test)
        {
            Debug.Log($"Test {test.BloonType.ToString()}");
        }
    }
}