using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public static class BloonsReferences
    {
        public static int PathLayer => LayerMask.NameToLayer("BloonPath");
        public static int OutOfBoundsLayer => LayerMask.NameToLayer("OutOfBounds");
        public static int TowerLayer => LayerMask.NameToLayer("Towers");
        public static int BloonsLayer => LayerMask.NameToLayer("Bloons");
        public static int HazardsLayer => LayerMask.NameToLayer("Hazards");
    }
}