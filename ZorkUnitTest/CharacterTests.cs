using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Zork;
using Zork.Characters;
using Zork.Objects;

namespace ZorkUnitTest
{
    [TestClass]
    public class CharacterTests
    {
        /// <summary>
        /// Tests if a character is being constructed with all the right parameters.
        /// </summary>
        [TestMethod]
        public void CharacterConstructorTest()
        {
            Weapon longSword = CreateWeapon();
            Character character1 = CreateNPC();
            Assert.AreEqual("constable_barney", character1.Name);
            Assert.AreEqual(4, character1.Strength);
            Assert.AreEqual(100, character1.Health);
            Assert.IsNotNull(character1.EquippedWeapon);
        }

        [TestMethod]
        public void NonExistingCharacterTest()
        {
            NPC npc = new NPC("sdqoiwqjd", "Highly valuable person", 0, 10, 5, null);
            Assert.IsTrue(npc.Text.RootNodes == null);
        }

        /// <summary>
        /// Tests if it is possible to have a character without location
        /// </summary>
        [TestMethod]
        public void CharacterWithoutWeaponTest()
        {
            Character character1 = CreateCharacterWithoutWeapon();
            Assert.IsNull(character1.EquippedWeapon);
        }

        /// <summary>
        /// creates a room with objects then calls the character to look around in this room and tests if the character sees the objects
        /// objects created are 3x createWeapon, then this method checks if the lookAroundText contains the right text to be printed to the console.
        /// </summary>
        [TestMethod]
        public void LookAroundTest()
        {
            Room room = new Room("A place", new Point(0,0));
            NPC npc = CreateNPC();
            room.NPCsInRoom.Add(npc);
            room.ObjectsInRoom = CreateListOfThreeWeaponObjects();
            using (StringWriter consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                room.PrintRoomContents();
                string lookAroundTextString = consoleOutput.ToString();
                string[] lookAroundTextList = lookAroundTextString.Split('\n');
                //check whether the second line of text contains the name of the character added to the room
                Assert.IsTrue(lookAroundTextList[0].Contains(npc.Name.Replace('_', ' ')));
                //checks for the descriptions of the three weapons (the objects in the room)
                for (int i = 3; i < 6; i++)
                {
                    Assert.IsTrue(lookAroundTextList[i].Contains(CreateWeapon().Description), "The objects do not match the right description");
                }
            }
        }
        [TestMethod]
        public void PrintInventoryTest()
        {
            Player player = CreatePlayerCharacter();
            using (StringWriter consoleOutput = new StringWriter())
            {

                Console.SetOut(consoleOutput);
                player.Inventory = new List<BaseObject>();
                Clue clue = new Clue("Red pants", "very nice pants");
                player.Inventory.Add(clue);
                player.PrintInventory();
                Assert.IsTrue(consoleOutput.ToString().Contains($"{clue.Name} {clue.Description}"));
            }
        }

        [TestMethod]
        public void KillNPCTest()
        {
            Game game = new Game();
            NPC npc = CreateNPC();
            npc.CurrentRoom = new Room("", new Point(0, 0));
            Room npcRoom = npc.CurrentRoom;
            npc.KillThisNPC(game);
            Assert.IsFalse(npcRoom.NPCsInRoom.Count > 0);
        }

        [TestMethod]
        public void TestIfNPCBecomesCorpseObject()
        {
            Game game = new Game();
            NPC npc = CreateNPC();
            npc.CurrentRoom = new Room("", new Point(0, 0));
            Room npcRoom = npc.CurrentRoom;
            npc.KillThisNPC(game);
            Assert.IsFalse(npcRoom.ObjectsInRoom[0] is CorpseNPCObject);
        }

        /// <summary>
        /// Creates a weapon objects for testing purposes
        /// </summary>
        /// <returns></returns>
        public static Weapon CreateWeapon()
        {
            return new Weapon("Longsword", 16, "a heavy longsword");
           
        }

        /// <summary>
        /// Creates a character holding a weapon for testing purposes
        /// </summary>
        /// <returns>A character equipped with a longsword</returns>
        public static NPC CreateNPC()
        {
            return new NPC("constable_barney", "This man has a long beard.", 4, 100, 5, CreateWeapon());
        }

        private List<BaseObject> CreateListOfThreeWeaponObjects()
        {
            List<BaseObject> objectsInRoom = new List<BaseObject>
            {
                CreateWeapon(),
                CreateWeapon(),
                CreateWeapon()
            };
            return objectsInRoom;
        }

        private Character CreateCharacterWithoutWeapon()
        {
            return new NPC("constable_barney", "This man has a long beard.", 4, 100, 5, null);
        }
        
        [TestMethod]
        public void CharactersMoveAround()
        {
            Game game = new Game();
            Dictionary<string, bool> characterHasMoved = InitialiseCharacterHasMoved(game.NPCS);
            Dictionary<string, Point> startLocations = GetCharacterLocations(game);
            for (int i = 0; i < NPC.MaxTurnsBetweenMoves; i++)
            {
                foreach(NPC npc in game.NPCS)
                {
                    npc.OnPlayerMoved(game);
                    if(npc.CurrentRoom.LocationOfRoom != startLocations[npc.Name])
                    {
                        characterHasMoved[npc.Name] = true;
                    }
                }
            }
            foreach (bool charMoved in characterHasMoved.Values)
            {
                Assert.IsTrue(charMoved);
            }
        }
        
        private static Dictionary<string, Point> GetCharacterLocations(Game game)
        {
            return game.NPCS.ToDictionary(x => x.Name, x => x.CurrentRoom.LocationOfRoom);
        }

        private static Dictionary<string, bool> InitialiseCharacterHasMoved(List<NPC> npcs)
        {
            return npcs.ToDictionary(x => x.Name, x => false);
        }

        public static Player CreatePlayerCharacter()
        {
            Player p = new Player(new Zork.Room("", new System.Drawing.Point(0, 0)))
            {
                EquippedWeapon = new Weapon("gun", 1, "description")
            };
            return p;
        }

        
    }
}
