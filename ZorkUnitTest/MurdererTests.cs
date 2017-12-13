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
            
            MurdererNPC murderer = CreateMurderer();
            MurdererNPC npc = CreateMurderer();
            murderer.CurrentRoom.NPCsInRoom.Add(npc);
            npc.CurrentRoom = murderer.CurrentRoom;
            int countNpcsInRoom = murderer.CurrentRoom.NPCsInRoom.Count;
            murderer.KillRandomNPCInSameRoom();
            Assert.IsTrue(countNpcsInRoom > murderer.CurrentRoom.NPCsInRoom.Count);

        }

        [TestMethod]
        public void MurdererCanKillNoOneIfRoomIsEmpty()
        {
            MurdererNPC murderer = CreateMurderer();
            murderer.CurrentRoom.NPCsInRoom.Clear();
            Assert.IsFalse(murderer.KillRandomNPCInSameRoom());
        }

        [TestMethod]
        public void MurdererTriesToKillEveryXTurns()
        {
            Maze m = new Maze(1,1,0,0);
            MurdererNPC murderer = CreateMurderer();
            murderer.maze = m;
            murderer.CurrentRoom = m.Rooms[0,0];
            murderer.CurrentRoom.NPCsInRoom.Add(CreateMurderer());
            int totalNpcs = 0;
            foreach (Room r in m.Rooms)
            {
                totalNpcs += r.NPCsInRoom.Count;
            }
            bool isLooped = false;
            while (murderer.StepsBeforeNextKill >= 0 && isLooped == false)
            {
                murderer.OnPlayerMoved();
                if(murderer.StepsBeforeNextKill == murderer.KillEveryXPlayerSteps)
                {
                    isLooped = true;
                }
            }

            int totalnpcsAfterKill = 0;
            foreach (Room r in m.Rooms)
            {
                totalnpcsAfterKill += r.NPCsInRoom.Count;
            }
            Assert.IsTrue(totalnpcsAfterKill < totalNpcs);
        }

        public MurdererNPC CreateMurderer()
        {
            MurdererNPC murderer = new Zork.Characters.MurdererNPC("Murderer", "desc", 10, 100, 99, null);
            murderer.CurrentRoom = new Room("", new System.Drawing.Point(0,0));
            return murderer;
            
        }

        public Maze createMaze()
        {
            return new Maze(5, 5, 0, 0);
        }
    }
}
