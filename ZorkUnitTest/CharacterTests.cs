using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using Zork.Objects;
using System.Drawing;
using System.Collections.Generic;

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
            Weapon longSword = createWeapon();
            Character character1 = new Character("Jan", 4, 100, longSword, new Room("A small room"), "This man has a long beard.");
            if (character1.Name != "Jan")
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
        /// <summary>
        /// Tests if it is possible to have a character without location
        /// </summary>
        [TestMethod]
        public void characterWithoutWeaponTest()
        {
            Character character1 = createCharacterWithoutWeapon();
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
        public void lookAroundTest()
        {
            Character character = createCharacter();
            character.Location.ObjectsInRoom = createListOfThreeWeaponObjects();
            character.Location.CharactersInRoom.Add(createCharacter());
            string lookAroundTextString = character.lookAround();
            string[] lookAroundTextList = lookAroundTextString.Split('\n');

            if (!lookAroundTextList[0].Contains(character.Location.Description))
            {
                Assert.Fail("The first object seen is not the room description, should start with room description");
            }
            //checks whether the second line of text contains the name of the character (the character which is added to the room
            if (!lookAroundTextList[2].Contains(character.Name))
            {
                Assert.Fail("The character's name is not printed in the second line of the look around method");
            }

            //checks if lines 4, 5, 6 are the description of the three weapons (the objects in the room)
            for (int i = 4; i < 7; i++)
            {
                Assert.IsTrue(lookAroundTextList[i].Contains(createWeapon().Description), "The objects do not match the right description");
            }
        }

        /// <summary>
        /// Creates a weapon objects for testing purposes
        /// </summary>
        /// <returns></returns>
        private Weapon createWeapon()
        {
            return new Weapon("Longsword", 16, "a heavy longsword");
           
        }

        /// <summary>
        /// Creates a character holding a weapon for testing purposes
        /// </summary>
        /// <returns>A character equipped with a longsword</returns>
        private Character createCharacter()
        {
            return new Character("Jan", 4, 100, createWeapon(), new Room("A golden room"), "This man has a long beard.");
        }

        private List<IObject> createListOfThreeWeaponObjects()
        {
            List<IObject> objectsInRoom = new List<IObject>();
            objectsInRoom.Add(createWeapon());
            objectsInRoom.Add(createWeapon());
            objectsInRoom.Add(createWeapon());
            return objectsInRoom;
        }

        private Character createCharacterWithoutWeapon()
        {
            return new Character("Jan", 4, 100, null, new Room("A golden room"), "This man has a long beard.");
        }
    }
}
