using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using Zork.Objects;
using Zork.Characters;
using System.IO;
using System.Drawing;

namespace ZorkUnitTest
{
    [TestClass]
    public class InteractionTests
    {
        [TestMethod]
        public void ChooseEnemyTest()
        {
            //TODO: ask how to test private methods and user input from console. (console.readlines)
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            Point currentRoom = new Point(0, 0);
            Maze maze = new Maze(1, 1, currentRoom.X, currentRoom.Y);
            maze[currentRoom.X, currentRoom.Y].NPCsInRoom.Add(CharacterDefinitions.NPCS[0]);
            Interactions.ChooseEnemyMessage(maze, currentRoom);
            Assert.IsTrue(writer.ToString().Contains($"[{0 + 1}] {maze[currentRoom].NPCsInRoom[0].Name}"));
        }

        [TestMethod]
        public void FightStrongEnemyTest()
        {
            //TODO: ask how to test private methods
            Assert.IsFalse(Interactions.Fight(new NPC("sherrif_barney", "desc", 30, 100, new Weapon("Strong weapon", 10, "desc")), new Player()));
        }

        [TestMethod]
        public void FightweakEnemyTest()
        {
            //TODO: ask how to test private methods
            Assert.IsTrue(Interactions.Fight(new NPC("sherrif_barney", "desc", 1, 10, null), new Player()));
        }
    }
}
