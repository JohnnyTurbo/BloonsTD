using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using TMG.BloonsTD.Stats;
using TMG.BloonsTD.TestInfrastructure;
using UnityEngine;

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
                .ThatSpawnsBloon(bloonPropertiesA).WithBloonType(BloonTypes.Blue);
            BloonProperties bloonPropertiesC =
                A.BloonProperties.ThatSpawnsBloon(bloonPropertiesB).WithBloonType(BloonTypes.Green);
            bloonPropertiesC.RedBloonEquivalent.Should().Be(4);
        }

        [Test]
        public void Test_Total_Bloon_Count_With_1_HP()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(1);
            bloonPropertiesA.TotalBloonCount.Should().Be(1);
        }
        
        [Test]
        public void Test_Total_Bloon_Count_With_2_HP()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(2);
            bloonPropertiesA.TotalBloonCount.Should().Be(1);
        }
        
        [Test]
        public void Test_Total_Bloon_Count_On_Bloon_That_Spawns_1_Bloon_With_1_HP()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(1);
            BloonProperties bloonPropertiesB = A.BloonProperties.WithHitsToPop(1).ThatSpawnsBloon(bloonPropertiesA);
            bloonPropertiesB.TotalBloonCount.Should().Be(2);
        }
        
        [Test]
        public void Test_Total_Bloon_Count_On_Bloon_That_Spawns_1_Bloon_With_2_HP()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(1);
            BloonProperties bloonPropertiesB = A.BloonProperties.WithHitsToPop(2).ThatSpawnsBloon(bloonPropertiesA);
            bloonPropertiesB.TotalBloonCount.Should().Be(2);
        }
        
        [Test]
        public void Test_Total_Bloon_Count_On_Bloon_That_Spawns_2_Bloons_With_1_HP()
        {
            BloonProperties bloonPropertiesA = A.BloonProperties.WithHitsToPop(1);
            BloonProperties bloonPropertiesB = A.BloonProperties.WithHitsToPop(1).ThatSpawnsBloon(bloonPropertiesA)
                .ThatSpawnsBloon(bloonPropertiesA);
            bloonPropertiesB.TotalBloonCount.Should().Be(3);
        }

        [Test]
        public void Test_Child_Bloon_Validation()
        {
            var bloonPropertiesA = A.BloonProperties;
            Action act = () => A.BloonProperties.ThatSpawnsBloon(bloonPropertiesA).WithBloonType(BloonTypes.Blue);
            act.Should().NotThrow<ArgumentException>();
        }
        
        [Test]
        public void Test_Incorrect_Child_Bloon_Validation()
        {
            var bloonPropertiesA = A.BloonProperties.WithBloonType(BloonTypes.Red);
            var bloonPropertiesB = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesA).WithBloonType(BloonTypes.Blue);
            Action act = () => A.BloonProperties.ThatSpawnsBloon(bloonPropertiesB).WithBloonType(BloonTypes.Blue);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Test_Changing_To_Valid_Child()
        {
            var bloonPropertiesA = A.BloonProperties.WithBloonType(BloonTypes.Red);
            var bloonPropertiesB = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesA).WithBloonType(BloonTypes.Blue);
            var bloonPropertiesC = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesB).WithBloonType(BloonTypes.Green);
            Action act = () => bloonPropertiesC.BloonsToSpawnWhenPopped = new List<BloonProperties>() {bloonPropertiesA};
            act.Should().NotThrow<ArgumentException>();
            bloonPropertiesC.BloonsToSpawnWhenPopped.Should().Equal(new List<BloonProperties>() {bloonPropertiesA});
        }
        
        [Test]
        public void Test_Changing_To_Invalid_Child()
        {
            var bloonPropertiesA = A.BloonProperties.WithBloonType(BloonTypes.Red);
            var bloonPropertiesB = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesA).WithBloonType(BloonTypes.Blue);
            var bloonPropertiesC = A.BloonProperties.ThatSpawnsBloon(bloonPropertiesB).WithBloonType(BloonTypes.Green);
            Action act = () => bloonPropertiesB.BloonsToSpawnWhenPopped = new List<BloonProperties>() {bloonPropertiesC};
            act.Should().Throw<ArgumentException>();
            bloonPropertiesB.BloonsToSpawnWhenPopped.Should().Equal(new List<BloonProperties>(){bloonPropertiesA});
        }
    }
}
