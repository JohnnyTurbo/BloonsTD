using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    public enum TowerSpeed
    {
        Slow,
        Medium,
        Fast,
        Hypersonic
    };
    
    [CreateAssetMenu(fileName = "TowerProperties", menuName = "ScriptableObjects/Tower Properties", order = 0)]
    public class TowerProperties : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _cost;
        [SerializeField] private TowerSpeed _speed;
        [TextArea][SerializeField] private string _description;

        public string Name => _name;
        public int Cost => _cost;

        public string Speed
        {
            get
            {
                switch (_speed)
                {
                    case TowerSpeed.Slow:
                        return "Slow";

                    case TowerSpeed.Medium:
                        return "Medium";

                    case TowerSpeed.Fast:
                        return "Fast";

                    case TowerSpeed.Hypersonic:
                        return "Hypersonic";

                    default:
                        return "Undefined";
                }
            }
        }

        public string Description => _description;
    }
}