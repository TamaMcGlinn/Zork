using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Zork;
using Zork.Behaviour;
using Zork.Characters;
using Zork.Objects;

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
            List<NPC> NPCs = (new CharacterDefinitions()).NPCS;
            maze[currentRoom.X, currentRoom.Y].CharactersInRoom.Add(NPCs[0]);
            Interactions i = new Interactions();
            i.ChooseEnemyMessage(maze, currentRoom);
            Assert.IsTrue(writer.ToString().Contains($"[{0 + 1}] {maze[currentRoom].CharactersInRoom[0].Name}"));
        }

        [TestMethod]
        public void FightStrongEnemyTest()
        {
            NPC npc = new NPC("sherrif_barney", 10, 100, new Weapon("Strong weapon", 10, "desc"), "", 5);
            NPC npc1 = new NPC("sherrif_barney", 60, 100, new Weapon("Strong weapon", 10, "desc"), "", 5);
            npc.Fight(npc1, CreateMaze().Rooms);
            Assert.IsTrue(npc.Health <= 0);
        }

        [TestMethod]
        public void FightweakEnemyTest()
        {
            NPC npc = new NPC("sherrif_barney", 30, 100, new Weapon("Strong weapon", 10, "desc"), "", 5);
            NPC npc1 = new NPC("sherrif_barney", 1, 10, new Weapon("Strong weapon", 10, "desc"), "", 5);
            npc.Fight(npc1, CreateMaze().Rooms);
            Assert.IsTrue(npc.Health > 0);
        }

        public Maze CreateMaze()
        {

            return new Maze(10, 10, 0, 0);
        }
    }
}
