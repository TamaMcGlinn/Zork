using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using Zork.Objects;
using System.Drawing;

namespace ZorkUnitTest
{
    [TestClass]
    public class CharacterTests
    {
        /// <summary>
        /// Tests if a character is being constructed with all the right parameters.
        /// </summary>
        [TestMethod]
        public void CharacterConstructorTest()
        {
            Weapon longSword = new Weapon("Longsword", 16, "A heavy longsword.");
            Character character1 = new Character("Jan", 4, 100, longSword, new Point(10,5));
            if (character1.Name != "Jan")
            {
                Assert.Fail("The name of the character is not correct");
            }

            if(character1.Strength != 4)
            {
                Assert.Fail("The strength is not correct");
            }

            if (character1.Health != 100)
            {
                Assert.Fail("The health of the character is not correct");
            }

            if (character1.EquippedWeapon == null)
            {
                Assert.Fail("The weapon of the character is not correct");
            }

            if(character1.Location.X != 10)
            {
                Assert.Fail("The X location of the character is not correct");
            }

            if (character1.Location.Y != 5)
            {
                Assert.Fail("The Y location of the character is not correct");
            }
        }
        /// <summary>
        /// Tests if it is possible to have a character without location
        /// </summary>
        [TestMethod]
        public void CharacterWithoutWeapon()
        {
            Character character1 = new Character("piet", 1, 100, null, new Point(0,0));
            if (character1.EquippedWeapon != null)
            {
                Assert.Fail("Somehow the character has a weapon");
            }
        }
    }
}
