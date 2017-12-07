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
            //TODO: ask how to test private methods and user input from console. (console.readlines)
            Interactions.ChooseEnemy(new Maze(1,1,0,0), new System.Drawing.Point(0,0));
            Assert.Fail();
        }

        [TestMethod]
        public void FightStrongEnemyTest()
        {
            //TODO: ask how to test private methods
            Assert.IsFalse(Interactions.Fight(new NPC("sherrif_barney", 30, 100, new Weapon("Strong weapon", 10, "desc"), "desc"), new Player()));
        }

        [TestMethod]
        public void FightweakEnemyTest()
        {
            //TODO: ask how to test private methods
            Assert.IsTrue(Interactions.Fight(new NPC("sherrif_barney", 1, 10, null, "desc"), new Player()));
        }
    }
}
