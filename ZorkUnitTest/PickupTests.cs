using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void HealthLimited()
        {
            var health = makeVial();
            Player p = new Player();
            p.UseHealthPickup(health);
            Assert.AreEqual(p.Health, 100);
        }

        [TestMethod]
        public void CanHeal()
        {
            var health = makeVial();
            Player p = new Player();
            p.takeDamage(50);
            p.UseHealthPickup(health);
            Assert.AreEqual(p.Health, 80);
        }

        [TestMethod]
        public void ClueAddsDescription()
        {
            Clue c = new Clue("parchment", "it says the man is Barry");

        }
    }
}
