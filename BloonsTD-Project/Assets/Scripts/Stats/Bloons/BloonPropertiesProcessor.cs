using System.Collections.Generic;
using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    public static class BloonPropertiesProcessor
    {
        private static Dictionary<BloonTypes, BloonProperties> _bloons = new Dictionary<BloonTypes, BloonProperties>();
        private static bool _initialized;

        private static void Initialize()
        {
            _bloons.Clear();
            Resources.LoadAll("ScriptableObjects");
            if (Resources.FindObjectsOfTypeAll(typeof(BloonProperties)) is BloonProperties[] allBloonProperties)
            {
                foreach (var bloonProperties in allBloonProperties)
                {
                    _bloons.Add(bloonProperties.BloonType, bloonProperties);
                } 
            }
            _initialized = true;
        }

        public static BloonProperties GetBloonPropertiesFromBloonType(BloonTypes bloonType)
        {
            
            if(!_initialized){Initialize();}
            
            return _bloons[bloonType];
        }
    }
}