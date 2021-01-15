using UnityEngine;

namespace TMG.BloonsTD.Stats
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Tower Upgrade", order = 0)]
    public class TowerUpgradeProperties : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private int _cost;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _upgradeIcon;

        
        //TODO: Custom inspector to show/hide value/object based on selected enum.
        [SerializeField] private float _upgradeValue;
        [SerializeField] private GameObject _weapon;

        public string Name => _name;
        public UpgradeType UpgradeType => _upgradeType;
        public int Cost => _cost;
        public string Description => _description;
        public Sprite UpgradeIcon => _upgradeIcon;
        public float UpgradeValue => _upgradeType == UpgradeType.Weapon ? float.NaN : _upgradeValue;
        public GameObject Weapon =>  _upgradeType == UpgradeType.Weapon ? _weapon : null;
    }
}