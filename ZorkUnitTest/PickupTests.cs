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
        private HealthPickup MakeVial()
        {
            return new HealthPickup("Vial", 30, "A bright green liquid in a thick glass vial.");
        }

        [TestMethod]
        public void MakeHealthPickup()
        {
            var health = MakeVial();
            Assert.IsNotNull(health);
        }

        [TestMethod]
        public void HealthLimited()
        {
            var health = MakeVial();
            Player p = CreatePlayer();
            p.UseHealthPickup(health);
            Assert.AreEqual(p.Health, 100);
        }

        [TestMethod]
        public void CanHeal()
        {
            var health = MakeVial();
            Player p = CreatePlayer();
            p.TakeDamage(50);
            p.UseHealthPickup(health);
            Assert.AreEqual(p.Health, 80);
        }

        [TestMethod]
        public void HealObjectGetsRemovedAfterUsingTest()
        {
            var health = MakeVial();
            Player p = CreatePlayer();
            p.TakeDamage(50);
            p.Inventory.Add(health);
            p.UseHealthPickup(health);
            Assert.IsFalse(p.Inventory.Contains(health));
        }

        [TestMethod]
        public void ClueAddsDescription()
        {
            Clue c = new Clue("parchment", "it says the man is Barry");

        }

        [TestMethod]
        public void PickUpWeaponTest()
        {
            Player p = CreatePlayer();
            Weapon w = new Weapon("Longsword", 5, "a big sword");
            w.PickupObject(p);
            Assert.IsTrue(p.EquippedWeapon == w);
        }

        [TestMethod]
        public void PickupObjectTest()
        {
            Player p = CreatePlayer();
            Clue bo = new Clue("pants", "description");
            bo.PickupObject(p);
            Assert.IsTrue(p.Inventory.Contains(bo));
        }

        [TestMethod]
        public void TestClues()
        {
            string clue = "c";
            Player p = CreatePlayer();
            p.Clues.Add(clue);
            Assert.IsTrue(p.Clues.Contains(clue));
        }

        public Player CreatePlayer()
        {
            return new Player(new Zork.Room("", new System.Drawing.Point(0, 0))); ;
        }
    }
}
