using System.Collections.Generic;
using TMG.BloonsTD.Stats;
using UnityEngine;

namespace TMG.BloonsTD.TestInfrastructure
{
    public class BloonPropertiesBuilder
    {
        private bool _canBePoppedByDarts = true;
        private bool _canBePoppedByTacks = true;
        private bool _canBePoppedByBombs = true;
        private bool _canBeFrozen = true;
        private bool _canBeDetected = true;
        private int _numberOfHitsToPop = 1;
        private float _moveSpeed = 1f;
        private BloonTypes _bloonType = BloonTypes.Red;
        private List<BloonProperties> _bloonsToSpawnWhenPopped;
        
        public BloonPropertiesBuilder CannotPopFromDarts()
        {
            _canBePoppedByDarts = false;
            return this;
        }

        public BloonPropertiesBuilder CannotPopFromTacks()
        {
            _canBePoppedByTacks = false;
            return this;
        }

        public BloonPropertiesBuilder CannotPopFromBombs()
        {
            _canBePoppedByBombs = false;
            return this;
        }

        public BloonPropertiesBuilder CannotBeFrozen()
        {
            _canBeFrozen = false;
            return this;
        }

        public BloonPropertiesBuilder CannotBeDetected()
        {
            _canBeDetected = false;
            return this;
        }
        
        public BloonPropertiesBuilder WithHitsToPop(int numberOfHitsToPop)
        {
            _numberOfHitsToPop = numberOfHitsToPop;
            return this;
        }

        public BloonPropertiesBuilder WithMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
            return this;
        }

        public BloonProperties WithBloonType(BloonTypes bloonType)
        {
            _bloonType = bloonType;
            return this;
        }
        
        public BloonPropertiesBuilder ThatSpawnsBloon(BloonProperties bloonToSpawnWhenPopped)
        {
            if(_bloonsToSpawnWhenPopped == null){_bloonsToSpawnWhenPopped = new List<BloonProperties>();}
            _bloonsToSpawnWhenPopped.Add(bloonToSpawnWhenPopped);
            return this;
        }
        
        public BloonPropertiesBuilder ThatSpawnsBloons(IEnumerable<BloonProperties> bloonsToSpawnWhenPopped)
        {
            _bloonsToSpawnWhenPopped = new List<BloonProperties>(bloonsToSpawnWhenPopped);
            return this;
        }

        private BloonProperties Build()
        {
            var newBloonProperties = ScriptableObject.CreateInstance<BloonProperties>();
            newBloonProperties.CanBePoppedByDarts = _canBePoppedByDarts;
            newBloonProperties.CanBePoppedByTacks = _canBePoppedByTacks;
            newBloonProperties.CanBePoppedByBombs = _canBePoppedByBombs;
            newBloonProperties.CanBeFrozen = _canBeFrozen;
            newBloonProperties.CanBeDetected = _canBeDetected;
            newBloonProperties.MoveSpeed = _moveSpeed;
            newBloonProperties.NumberOfHitsToPop = _numberOfHitsToPop;
            newBloonProperties.BloonType = _bloonType;
            if (_bloonsToSpawnWhenPopped != null)
            {
                newBloonProperties.BloonsToSpawnWhenPopped = new List<BloonProperties>(_bloonsToSpawnWhenPopped);
            }
            return newBloonProperties;
        }

        public static implicit operator BloonProperties(BloonPropertiesBuilder builder)
        {
            return builder.Build();
        }
        
    }
}