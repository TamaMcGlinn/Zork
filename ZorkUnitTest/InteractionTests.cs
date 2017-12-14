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
            Point currentRoom = new Point(0, 0);
            Maze maze = new Maze(1, 1, currentRoom.X, currentRoom.Y);
            Player p = new Player(maze.Rooms[currentRoom.X,currentRoom.Y]);
            NPC npc = createNPCBarney();
            maze[currentRoom.X, currentRoom.Y].NPCsInRoom.Add(npc);
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            p.CurrentRoom.PrintNPCs();
            Assert.IsTrue(consoleOutput.ToString().Contains($"[1] {maze[currentRoom].NPCsInRoom[0].Name.Replace('_',' ')}"));
        }

        [TestMethod]
        public void FightStrongEnemyTest()
        {
            Game game = new Game();
            Maze maze = CreateMaze();
            Room room = maze.Rooms[0, 0];
            Player player = new Player(room);
            NPC barney = createNPCBarney();
            player.Fight(barney, game);
            Assert.IsTrue(player.Health <= 0);
        }

        [TestMethod]
        public void FightweakEnemyTest()
        {
            Game game = new Game();
            Maze maze = CreateMaze();
            Room room = maze.Rooms[0, 0];
            Player player = new Player(room);
            NPC barney = CreateWeakNPC();
            player.Fight(barney, game);
            Assert.IsTrue(player.Health > 0);
        }

        public Maze CreateMaze()
        {

            return new Maze(10, 10, 0, 0);
        }

        public NPC createNPCBarney()
        {

            Room r = new Room("", new Point(0, 0));
            NPC barney = new NPC("sherrif_barney", "", 30, 100, 5, new Weapon("Strong weapon", 10, "desc"));
            barney.CurrentRoom = r;
            r.NPCsInRoom.Add(barney);
            return barney;
        }

        public NPC CreateWeakNPC()
        {
           NPC npc =  new NPC("sherrif_barney", "", 1, 10, 5, new Weapon("Strong weapon", 10, "desc"));
            Room r = new Room("", new Point(0, 0));
            npc.CurrentRoom = r;
            r.NPCsInRoom.Add(npc);
            return npc;
        }

    }
}
