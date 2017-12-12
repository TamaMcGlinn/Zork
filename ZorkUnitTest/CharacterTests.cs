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
    public class CharacterTests
    {
        /// <summary>
        /// Tests if a character is being constructed with all the right parameters.
        /// </summary>
        [TestMethod]
        public void characterConstructorTest()
        {
            Weapon longSword = CreateWeapon();
            Character character1 = CreateNPC();
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
            NPC npc = new NPC("sdqoiwqjd", "Highly valuable person", 0, 10, 5, null, false);
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
            CharacterDefinitions characters = new CharacterDefinitions();
            Room room = new Room("A place", new System.Drawing.Point(0,0));
            Character character = characters.NPCS[0];
            room.NPCsInRoom.Add(character as NPC);
            room.ObjectsInRoom = CreateListOfThreeWeaponObjects();
            string lookAroundTextString = room.PrintLookAroundString();
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
            CharacterDefinitions characters = createPlayerCharacter();
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            characters.PlayerCharacter.Inventory = new List<BaseObject>();
            Clue clue = new Clue("Red pants", "very nice pants");
            characters.PlayerCharacter.Inventory.Add(clue);
            characters.PlayerCharacter.PrintInventory();
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
            return new NPC("sherrif_barney", "This man has a long beard.", 4, 100, 5, CreateWeapon(), false);
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
            return new NPC("sherrif_barney", "This man has a long beard.", 4, 100, 5, null, false);
        }
        
        [TestMethod]
        public void CharactersMovesAround()
        {
            CharacterDefinitions cd = new CharacterDefinitions();
            Maze maze = new Maze(5, 5, 1, 1);
            cd.AddCharacters(maze);
            Dictionary<string, bool> characterHasMoved = InitialiseCharacterHasMoved();
            Dictionary<NPC, Point> startLocations = InitialiseStartLocations(maze);
            MoveCharactersAround(maze, characterHasMoved, startLocations, cd);
            foreach (bool charMoved in characterHasMoved.Values)
            {
                Assert.IsTrue(charMoved);
            }
        }

        private void MoveCharactersAround(Maze maze, Dictionary<string, bool> characterHasMoved, Dictionary<NPC, Point> startLocations, CharacterDefinitions cd)
        {
            for (int i = 0; i < NPC.MaxTurnsBetweenMoves; i++)
            {
                cd.MoveNPCs(maze);
                foreach (NPC c in cd.NPCS)
                {
                    if (!maze[startLocations[c]].NPCsInRoom.Contains(c))
                    {
                        characterHasMoved[c.Name] = true;
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

        private static Dictionary<string, bool> InitialiseCharacterHasMoved()
        {
            CharacterDefinitions cd = new CharacterDefinitions();
            Dictionary<string, bool> characterHasMoved = new Dictionary<string, bool>();
            foreach (Character c in cd.NPCS)
            {
                characterHasMoved.Add(c.Name, false);
            }
            return characterHasMoved;
        }
        public CharacterDefinitions createPlayerCharacter()
        {
            CharacterDefinitions characters = new CharacterDefinitions();
            Player p = new Player(new Zork.Room("", new System.Drawing.Point(0, 0)));
            p.EquippedWeapon = new Weapon("gun", 1, "description");
            characters.PlayerCharacter = p;
            return characters;
        }
    }
}
