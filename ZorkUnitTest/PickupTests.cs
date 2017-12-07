using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;
using Zork.Characters;
using Zork.Objects;

namespace ZorkUnitTest
{
    [TestClass]
    public class PickupTests
    {
        private HealthPickup makeVial()
        {
            return new HealthPickup("Vial", 30, "A bright green liquid in a thick glass vial.");
        }

        [TestMethod]
        public void MakeHealthPickup()
        {
            var health = makeVial();
            Assert.IsNotNull(health);
        }

        [TestMethod]
        public void HealthIncreases()
        {
            var health = makeVial();
            //Player p = new Player();
        }

        [TestMethod]
        public void ClueAddsDescription()
        {
            Clue c = new Clue("parchment", "it says the man is Barry");

        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void PickUpWeaponTest()
        {
            Player p = CharacterDefinitions.PlayerCharacter;
            Weapon w = new Weapon("Longsword", 5, "a big sword");
            p.PickUp(w);
            Assert.IsTrue(p.EquippedWeapon == w);
        }

        public void PickupObjectTest()
        {
            Player p = CharacterDefinitions.PlayerCharacter;
            Clue bo = new Clue("pants", "description");
            p.PickUp(bo);
            Assert.IsTrue(p.Inventory.Contains(bo));
        }
    }
}
