using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
