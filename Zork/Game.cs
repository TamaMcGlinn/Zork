using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Zork.Characters;
using Zork.Extensions;
using Zork.Objects;

namespace Zork
{
    /// <summary>
    /// A Zork Game, which initialises the rooms, characters and objects, and allows you to play.
    /// </summary>
    public class Game
    {
        Maze maze;
        Point currentRoom;
        const int Width = 20;
        const int Height = 20;
        const int StartX = 1;
        const int StartY = 1;
        Random rng = new Random();

        public Game()
        {
            currentRoom = new Point(StartX, StartY);
            maze = new Maze(Width, Height, StartX, StartY);
            maze.Print();
            CharacterDefinitions.AddCharacters(maze);
            ObjectDefinitions.AddItems(maze);
        }

        /// <summary>
        /// Attempt to go from the from point to the towards point.
        /// </summary>
        /// <param name="from">From point</param>
        /// <param name="towards">Destination point</param>
        /// <param name="direction">Direction from from to towards</param>
        private void TryGo(Point from, Point towards, Direction direction)
        {
            if( towards.X < 0 || towards.X == Width || towards.Y < 0 || towards.Y == Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            } else if( maze[from].CanGoThere[direction])
            {
                currentRoom = towards;
            } else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }

        private void tryTalk(string userInput)
        {
            var talkCommand = userInput.Split(' ');
            if (talkCommand.Length >= 3 && talkCommand[1] == "to")
            {
                string charactername = string.Join("_", talkCommand.Skip(2)).ToLower();
                Character talkTarget = maze[currentRoom].CharactersInRoom.Find((Character c) => { return c.Name == charactername; });
                if (talkTarget == null)
                {
                    Console.WriteLine("There is nobody called " + charactername + " here.");
                }
                talkTarget.Talk();
            }
            else
            {
                Console.WriteLine("Did you mean; \"Talk to [character name]\"?");
            }
        }

        /// <summary>
        /// Print the room, get user input to accept commands
        /// </summary>
        public void Run()
        {
            PrintInstructions();
            while (true) {
                maze[currentRoom].Print();
                string userInput = Console.ReadLine();
                switch (userInput[0])
                {
                    case 'n':
                    case 'N':
                        TryGo(currentRoom, new Point(currentRoom.X, currentRoom.Y-1), Direction.North);
                        break;
                    case 's':
                    case 'S':
                        TryGo(currentRoom, new Point(currentRoom.X, currentRoom.Y + 1), Direction.South);
                        break;
                    case 'e':
                    case 'E':
                        TryGo(currentRoom, new Point(currentRoom.X + 1, currentRoom.Y), Direction.East);
                        break;
                    case 'w':
                    case 'W':
                        TryGo(currentRoom, new Point(currentRoom.X - 1, currentRoom.Y), Direction.West);
                        break;
                    case 'L':
                    case 'l':
                        Console.Write(maze[currentRoom].LookAround());
                        break;
                    case 't':
                    case 'T':
                        tryTalk(userInput);
                        break;
                    case 'p':
                    case 'P':
                        PickupItem();
                        break;
                    case 'i':
                    case 'I':
                        CharacterDefinitions.PlayerCharacter.PrintInventory();
                        break;
                    case 'c':
                    case 'C':
                        CharacterDefinitions.PlayerCharacter.PrintStats();
                        break;
                    default:
                        PrintInstructions();
                        break;
                }
            }
        }
        
        /// <summary>
        /// Lists all items in the room and gives options for the player to pick them up. 
        /// If he chooses a valid item it gets added to the inventory.
        /// </summary>
        private void PickupItem()
        {
            if (maze[currentRoom].ObjectsInRoom.Count <= 0)
            {
                Console.WriteLine("There are no items to pickup in this room.");
                return;
            }
            for (int i = 0; i < maze[currentRoom].ObjectsInRoom.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] to pickup:" + maze[currentRoom].ObjectsInRoom[i].Name);
            }
            string input = Console.ReadLine();
            int inputInteger;
            int.TryParse(input, out inputInteger);
            if (inputInteger > 0 && inputInteger < maze[currentRoom].ObjectsInRoom.Count + 1)
            {
                (maze[currentRoom].ObjectsInRoom[inputInteger - 1]).PickupObject(CharacterDefinitions.PlayerCharacter);
            }
            else
            {
                Console.WriteLine("Cannot pick that item up.");
            }
        }

        private void PrintInstructions()
        {
            Console.WriteLine("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around.");
            Console.WriteLine("[L] to look around, [P] to pick up an item, [I] Inventory");
            Console.WriteLine("[C] to view stats");
        }
    }
}
