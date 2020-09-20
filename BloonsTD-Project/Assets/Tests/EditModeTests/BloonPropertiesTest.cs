using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using FluentAssertions;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.TestInfrastructure;

namespace EditModeTests
{
    public class BloonPropertiesTest
    {
        [Test]
        public void Test_RBE_On_Bloon_With_1_HP()
        {
            BloonProperties redBloonProperties = A.BloonProperties.WithHitsToPop(1);
            redBloonProperties.RedBloonEquivalent.Should().Be(1);
        }

        [Test]
        public void Test_RBE_On_Bloon_With_2_HP()
        {
            BloonProperties redBloonProperties = A.BloonProperties.WithHitsToPop(2);
            redBloonProperties.RedBloonEquivalent.Should().Be(2);
        }
        
        [Test]
        public void Test_RBE_On_Bloon_That_Spawns_1_Bloon()
        {
            BloonProperties redBloonProperties = A.BloonProperties.WithHitsToPop(1);
            BloonProperties blueBloonProperties =
                A.BloonProperties.WithHitsToPop(1).ThatSpawnsBloon(redBloonProperties);
            blueBloonProperties.RedBloonEquivalent.Should().Be(2);
        }

        [Test]
        public void Test_RBE_On_Bloon_That_Spawns_Bloon_With_2_HP()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(2);
            BloonProperties bloonPropertiesB = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesA);
            bloonPropertiesB.RedBloonEquivalent.Should().Be(3);
        }
        
        [Test]
        public void Test_RBE_On_Bloon_That_Spawns_2_Bloons()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(1);
            BloonProperties bloonPropertiesB = A.BloonProperties.WithHitsToPop(1).ThatSpawnsBloon(bloonPropertiesA)
                .ThatSpawnsBloon(bloonPropertiesA);
            bloonPropertiesB.RedBloonEquivalent.Should().Be(3);
        }

        [Test]
        public void Test_RBE_Bloon_That_Spawns_Bloon_That_Spawns_2_Bloons()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(1);
            BloonProperties bloonPropertiesB = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesA)
                .ThatSpawnsBloon(bloonPropertiesA);
            BloonProperties bloonPropertiesC = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesB);
            bloonPropertiesC.RedBloonEquivalent.Should().Be(4);
        }
    }
}
