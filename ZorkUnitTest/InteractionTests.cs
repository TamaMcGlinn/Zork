using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Zork;
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
            Player p = new Player(maze.Rooms[currentRoom.X,currentRoom.Y]);
            List<NPC> NPCs = (new CharacterDefinitions()).NPCS;
            maze[currentRoom.X, currentRoom.Y].NPCsInRoom.Add(NPCs[0]);
            p.CurrentRoom.PrintAvailableEnemiesInRoom();
            Assert.IsTrue(writer.ToString().Contains($"[{0 + 1}] {maze[currentRoom].NPCsInRoom[0].Name}"));

        }

        [TestMethod]
        public void FightStrongEnemyTest()
        {
            NPC npc = CreateWeakNPC();
            NPC npc1 = createNPCBarney();
            npc.Fight(npc1, CreateMaze());
            Assert.IsTrue(npc.Health <= 0);
        }

        [TestMethod]
        public void FightweakEnemyTest()
        {
            NPC npc = createNPCBarney();
            NPC npc1 = CreateWeakNPC();
            npc.Fight(npc1, CreateMaze());
            Assert.IsTrue(npc.Health > 0);
        }

        public Maze CreateMaze()
        {

            return new Maze(10, 10, 0, 0);
        }

        public NPC createNPCBarney()
        {
            return new NPC("sherrif_barney", "", 30, 100, 5, new Weapon("Strong weapon", 10, "desc"));
        }

        public NPC CreateWeakNPC()
        {
            return new NPC("sherrif_barney", "", 1, 10, 5, new Weapon("Strong weapon", 10, "desc"));
        }

    }
}
