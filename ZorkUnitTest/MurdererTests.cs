using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork.Characters;
using Zork;
using Zork.Objects;
using System.Drawing;

namespace ZorkUnitTest
{
    [TestClass]
    public class MurdererTests
    {
        [TestMethod]
        public void TestMurdererCreation()
        {
            Assert.IsNotNull(CreateMurderer());
        }

        [TestMethod]
        public void DropWeaponTest()
        {
            MurdererNPC murderer =  CreateMurderer();
            murderer.EquippedWeapon = new Zork.Objects.Weapon("Blade", 10, "a powerfull blade");
            if (murderer.EquippedWeapon != null)
            {
                murderer.DropWeapon();
            }
            Assert.IsTrue(murderer.EquippedWeapon == null);
        }

        [TestMethod]
        public void DropAllItemsTest()
        {
            MurdererNPC murderer = CreateMurderer();
            murderer.Inventory.Add(new HealthPickup("drink", 100, "a nice drink"));
            murderer.Inventory.Add(new HealthPickup("drink", 100, "a nice drink"));
            murderer.Inventory.Add(new HealthPickup("drink", 100, "a nice drink"));
            murderer.Inventory.Add(new HealthPickup("drink", 100, "a nice drink"));
            if(murderer.Inventory.Count >= 4){
                murderer.DropAllItems();
            }
            Assert.IsTrue(murderer.Inventory.Count == 0);
        }

        [TestMethod]
        public void MurdererCanKillTest()
        {
            Game game = new Game();
            MurdererNPC murderer = CreateMurderer();
            MurdererNPC npc = CreateMurderer();
            murderer.CurrentRoom.NPCsInRoom.Add(npc);
            npc.CurrentRoom = murderer.CurrentRoom;
            int countNpcsInRoom = murderer.CurrentRoom.NPCsInRoom.Count;
            murderer.KillRandomNPCInSameRoom(game);
            Assert.IsTrue(countNpcsInRoom > murderer.CurrentRoom.NPCsInRoom.Count);
        }

        [TestMethod]
        public void MurdererCanKillNoOneIfRoomIsEmpty()
        {
            Game game = new Game();
            MurdererNPC murderer = CreateMurderer();
            murderer.CurrentRoom.NPCsInRoom.Clear();
            murderer.KillRandomNPCInSameRoom(game);
            Assert.IsTrue(murderer.CurrentRoom.NPCsInRoom.Count == 0);
        }

        [TestMethod]
        public void MurdererTriesToKillEveryXTurns()
        {
            Game game = new Game();
            Maze m = new Maze(1, 1, 0, 0);
            MurdererNPC murderer = CreateMurderer();
            murderer.Maze = m;
            murderer.CurrentRoom = m.Rooms[0, 0];
            NPC victim = CharacterTests.CreateNPC();
            victim.CurrentRoom = murderer.CurrentRoom;
            murderer.CurrentRoom.NPCsInRoom.Add(victim);
            murderer.CurrentRoom.NPCsInRoom.Add(murderer);
            int totalNpcs = CountNPCs(m);
            for (int i = 0; i < murderer.KillEveryXPlayerSteps; ++i)
            {
                murderer.OnPlayerMoved(game);
            }
            int totalnpcsAfterKill = CountNPCs(m);
            Assert.IsTrue(totalnpcsAfterKill < totalNpcs);
        }

        private static int CountNPCs(Maze m)
        {
            int totalNpcs = 0;
            foreach (Room r in m.Rooms)
            {
                totalNpcs += r.NPCsInRoom.Count;
            }

            return totalNpcs;
        }

        public MurdererNPC CreateMurderer()
        {
            MurdererNPC murderer = new Zork.Characters.MurdererNPC("Murderer", "desc", 10, 100, 99, null)
            {
                CurrentRoom = new Room("", new System.Drawing.Point(0, 0))
            };
            return murderer;
            
        }

        public Maze CreateMaze()
        {
            return new Maze(5, 5, 0, 0);
        }
    }
}
