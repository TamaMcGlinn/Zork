using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using Zork.Objects;
using System.Drawing;
using System.Collections.Generic;
using Zork.Characters;
using System.IO;

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
            Character character1 = new NPC("sherrif_barney", "This man has a long beard.", 4, 100, longSword);
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
            NPC character = CreateNPC();
            room.NPCsInRoom.Add(character);
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
            if (!lookAroundTextList[2].Contains(character.Name.Replace('_', ' ')))
            {
                Assert.Fail("The character's name is not printed in the second line of the look around method");
            }

            //checks if lines 4, 5, 6 are the description of the three weapons (the objects in the room)
            for (int i = 4; i < 7; i++)
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
        private NPC CreateNPC()
        {
            return new NPC("sherrif_barney", "This man has a long beard.", 4, 100, CreateWeapon());
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
            return new NPC("sherrif_barney", "This man has a long beard.", 4, 100);
        }
        
        [TestMethod]
        public void CharactersMovesAround()
        {
            Maze maze = new Maze(5, 5, 1, 1);
            CharacterDefinitions.AddCharacters(maze);
            Dictionary<Character, bool> characterHasMoved = InitialiseCharacterHasMoved();
            Dictionary<NPC, Point> startLocations = InitialiseStartLocations(maze);
            MoveCharactersAround(maze, characterHasMoved, startLocations);
            foreach (bool charMoved in characterHasMoved.Values)
            {
                Assert.IsTrue(charMoved);
            }
        }

        private static void MoveCharactersAround(Maze maze, Dictionary<Character, bool> characterHasMoved, Dictionary<NPC, Point> startLocations)
        {
            for (int i = 0; i < NPC.MaxTurnsBetweenMoves; i++)
            {
                CharacterDefinitions.MoveNPCs(maze);
                foreach (NPC c in CharacterDefinitions.NPCS)
                {
                    if (!maze[startLocations[c]].NPCsInRoom.Contains(c))
                    {
                        characterHasMoved[c] = true;
                    }
                }
            }
        }

        private static Dictionary<NPC, Point> InitialiseStartLocations(Maze maze)
        {
            Dictionary<NPC, Point> startLocations = new Dictionary<NPC, Point>();
            for (int xi = 0; xi < maze.Width; ++xi)
            {
                for (int yi = 0; yi < maze.Height; ++yi)
                {
                    foreach (NPC npc in maze[xi, yi].NPCsInRoom)
                    {
                        startLocations.Add(npc, new Point(xi, yi));
                    }
                }
            }
            return startLocations;
        }

        private static Dictionary<Character, bool> InitialiseCharacterHasMoved()
        {
            Dictionary<Character, bool> characterHasMoved = new Dictionary<Character, bool>();
            foreach (Character c in CharacterDefinitions.NPCS)
            {
                characterHasMoved.Add(c, false);
            }
            return characterHasMoved;
        }
    }
}
