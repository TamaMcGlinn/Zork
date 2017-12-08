using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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
        public void characterConstructorTest()
        {
            Weapon longSword = CreateWeapon();
            Character character1 = new NPC("sherrif_barney", 4, 100, longSword, "This man has a long beard.");
            if (character1.Name != "sherrif_barney")
            {
                Assert.Fail("The name of the character is not correct");
            }

            if(character1.Strength != 4)
            {
                Assert.Fail("The strength is not correct");
            }

            if (character1.Health != 100)
            {
                Assert.Fail("The health of the character is not correct");
            }

            if (character1.EquippedWeapon == null)
            {
                Assert.Fail("The weapon of the character is not correct");
            }
        }


        [TestMethod]
        public void NonExistingCharacterTest()
        {
            NPC npc = new NPC("sdqoiwqjd", "Highly valuable person");
            Assert.IsTrue(npc.Text.RootNode == null);
        }

        /// <summary>
        /// Tests if it is possible to have a character without location
        /// </summary>
        [TestMethod]
        public void characterWithoutWeaponTest()
        {
            Character character1 = CreateCharacterWithoutWeapon();
            if (character1.EquippedWeapon != null)
            {
                Assert.Fail("Somehow the character has a weapon");
            }
        }

        /// <summary>
        /// creates a room with objects then calls the character to look around in this room and tests if the character sees the objects
        /// objects created are 3x createWeapon, then this method checks if the lookAroundText contains the right text to be printed to the console.
        /// </summary>
        [TestMethod]
        public void LookAroundTest()
        {
            Room room = new Room("A place");
            Character character = CharacterDefinitions.NPCS[0];
            room.CharactersInRoom.Add(character);
            room.ObjectsInRoom = CreateListOfThreeWeaponObjects();
            string lookAroundTextString = room.LookAround();
            string[] lookAroundTextList = lookAroundTextString.Split('\n');

            //Na feedback dit:
            //checks if the current room's description is being printed first.
            Assert.IsTrue(lookAroundTextString.Contains(room.Description));
            //in plaats van:
            //checks if the current room's description is being printed first.
            //if (!lookAroundTextList[0].Contains(character.Location.Description))
            //{
            //    Assert.Fail("The first object seen is not the room description, should start with room description");
            //}
            //en voor de volgtijdelijkheid dan: assert.istrue(indexof(1) < indexof(2));


            //checks whether the second line of text contains the name of the character (the character which is added to the room
            if (!lookAroundTextList[3].Contains(character.Name.Replace('_', ' ')))
            {
                Assert.Fail("The character's name is not printed in the second line of the look around method");
            }

            //checks if lines 4, 5, 6 are the description of the three weapons (the objects in the room)
            for (int i = 6; i < 9; i++)
            {
                Assert.IsTrue(lookAroundTextList[i].Contains(CreateWeapon().Description), "The objects do not match the right description");
            }
        }
        [TestMethod]
        public void PrintInventoryTest()
        {
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            CharacterDefinitions.PlayerCharacter.Inventory = new List<BaseObject>();
            Clue clue = new Clue("Red pants", "very nice pants");
            CharacterDefinitions.PlayerCharacter.Inventory.Add(clue);
            CharacterDefinitions.PlayerCharacter.PrintInventory();
            Assert.IsTrue(consoleOutput.ToString().Contains($"{clue.Name} : {clue.Description}"));
        }

        /// <summary>
        /// Creates a weapon objects for testing purposes
        /// </summary>
        /// <returns></returns>
        private Weapon CreateWeapon()
        {
            return new Weapon("Longsword", 16, "a heavy longsword");
           
        }

        /// <summary>
        /// Creates a character holding a weapon for testing purposes
        /// </summary>
        /// <returns>A character equipped with a longsword</returns>
        private Character CreateCharacter()
        {
            return new NPC("sherrif_barney", 4, 100, CreateWeapon(), "This man has a long beard.");
        }

        private List<BaseObject> CreateListOfThreeWeaponObjects()
        {
            List<BaseObject> objectsInRoom = new List<BaseObject>();
            objectsInRoom.Add(CreateWeapon());
            objectsInRoom.Add(CreateWeapon());
            objectsInRoom.Add(CreateWeapon());
            return objectsInRoom;
        }

        private Character CreateCharacterWithoutWeapon()
        {
            return new NPC("sherrif_barney", 4, 100, null, "This man has a long beard.");
        }

    }
}
