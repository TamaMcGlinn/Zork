using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using Zork.Objects;
using Zork.Characters;

namespace ZorkUnitTest
{
    [TestClass]
    public class InteractionTests
    {
        [TestMethod]
        public void ChooseEnemyTest()
        {
            //TODO: ask how to test private methods
            Interactions i = new Interactions();
            i.ChooseEnemy(new Maze(1,1,0,0), new System.Drawing.Point(0,0));
            Assert.Fail();
        }

        [TestMethod]
        public void FightStrongEnemyTest()
        {
            //TODO: ask how to test private methods
            Interactions i = new Interactions();
            Assert.IsFalse(i.Fight(new NPC("sherrif_barney", 30, 100, new Weapon("Strong weapon", 10, "desc"), "desc")));
        }

        [TestMethod]
        public void FightweakEnemyTest()
        {
            //TODO: ask how to test private methods
            Interactions i = new Interactions();
            Assert.IsTrue(i.Fight(new NPC("sherrif_barney", 1, 50, null, "desc")));
        }
    }
}
