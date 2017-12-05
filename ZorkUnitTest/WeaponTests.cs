using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork.Objects;

namespace ZorkUnitTest
{
    [TestClass]
    public class WeaponTests
    {
        /// <summary>
        /// Tests if a weapon gets constructed with the right parameters
        /// </summary>
        [TestMethod]
        public void WeaponConstructorTest()
        {
            Weapon weapon = new Weapon("Longsword", 16, "A heavy longsword.");
            if(weapon.Name!="Longsword")
            {
                Assert.Fail("Weapon name is not correct");
            }
            if(weapon.Strength != 16)
            {
                Assert.Fail("Weapon strength is not correct");
            }

            if(weapon.Description != "A heavy longsword")
            {
                Assert.Fail("The description is not correct");
            }
        }
    }
}
