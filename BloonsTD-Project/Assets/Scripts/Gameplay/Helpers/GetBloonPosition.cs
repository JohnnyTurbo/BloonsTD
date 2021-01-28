using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public static class GetBloonPosition
    {
        public static Vector3 First(IReadOnlyList<BloonController> bloons)
        {
            var furthestBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                furthestBloon = BloonController.CompareGreaterPathProgress(furthestBloon, bloons[i]);
            }

            return furthestBloon.transform.position;
        }

        public static Vector3 Last(IReadOnlyList<BloonController> bloons)
        {
            var lastBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                lastBloon = BloonController.CompareLeastPathProgress(lastBloon, bloons[i]);
            }

            return lastBloon.transform.position;
        }

        public static Vector3 Strongest(IReadOnlyList<BloonController> bloons)
        {
            var strongestBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                strongestBloon = BloonController.CompareStrongest(strongestBloon, bloons[i]);
            }

            return strongestBloon.transform.position;
        }

        public static Vector3 Weakest(IReadOnlyList<BloonController> bloons)
        {
            var weakestBloon = bloons[0];
            for (var i = 1; i < bloons.Count; i++)
            {
                weakestBloon = BloonController.CompareWeakest(weakestBloon, bloons[i]);
            }

            return weakestBloon.transform.position;
        }

        public static Vector3 Closest(IReadOnlyList<BloonController> bloons, Vector3 towerPosition)
        {
            var closestBloon = bloons[0];
            var closestDistance = Vector3.Distance(towerPosition, closestBloon.transform.position);
            for (var i = 1; i < bloons.Count; i++)
            {
                var currentBloonDistance = Vector3.Distance(towerPosition, bloons[i].transform.position);
                var currentBloonIsCloserThanPreviousClosest = currentBloonDistance < closestDistance;

                if (!currentBloonIsCloserThanPreviousClosest) continue;

                closestBloon = bloons[i];
                closestDistance = currentBloonDistance;
            }

            return closestBloon.transform.position;
        }
    }
}