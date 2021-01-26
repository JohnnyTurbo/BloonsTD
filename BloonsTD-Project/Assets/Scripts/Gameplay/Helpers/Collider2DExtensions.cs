using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    public static class Collider2DExtensions
    {
        public static bool IsOnLayer(this Collider2D collider, int layer)
        {
            return collider.gameObject.layer.Equals(layer);
        }

        public static bool IsNotOnLayer(this Collider2D collider, int layer)
        {
            return !collider.gameObject.layer.Equals(layer);
        }
        public static bool IsNotABloon(this Collider2D other)
        {
            return !other.gameObject.layer.Equals(PhysicsLayers.Bloons);
        }
    }
}