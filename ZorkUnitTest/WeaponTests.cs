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
            string name = "Longsword";
            string desc = "A heavy longsword.";
            int strength = 16;

            Weapon weapon = new Weapon(name, strength, desc);
            Assert.AreEqual(name, weapon.Name);
            Assert.AreEqual(strength, weapon.Strength);
            Assert.AreEqual(desc, weapon.Description);
        }
    }
}
