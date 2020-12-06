using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public static class PhysicsLayers
    {
        public static int Path => LayerMask.NameToLayer("BloonPath");
        public static int OutOfBounds => LayerMask.NameToLayer("OutOfBounds");
        public static int Towers => LayerMask.NameToLayer("Towers");
        public static int Bloons => LayerMask.NameToLayer("Bloons");
        public static int Hazards => LayerMask.NameToLayer("Hazards");
    }
}