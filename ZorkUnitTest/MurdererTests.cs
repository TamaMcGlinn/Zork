using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork.Characters;
using Zork;
using Zork.Objects;

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
            CharacterDefinitions characters = new CharacterDefinitions();
            MurdererNPC murderer = CreateMurderer();
            murderer.CurrentRoom.NPCsInRoom.Add(characters.NPCS[0]);
            characters.NPCS[0].CurrentRoom = murderer.CurrentRoom;
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

        public MurdererNPC CreateMurderer()
        {
            MurdererNPC murderer = new Zork.Characters.MurdererNPC("Murderer", "desc", 10, 100, 99, null);

            murderer.CurrentRoom = new Room("", new System.Drawing.Point(0,0));
            return murderer;
            
        }
    }
}
