using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Zork.Characters;
using Zork.Objects;

namespace Zork
{
    /// <summary>
    /// A Zork Game, which initialises the rooms, characters and objects, and allows you to play.
    /// </summary>
    public class Game
    {
        Maze maze;
        public Point currentRoom;
        public const int Width = 2;
        public const int Height = 2;
        const int StartX = 1;
        const int StartY = 1;
        Interactions interactions = new Interactions();
        CharacterDefinitions characters = new CharacterDefinitions();
        Dictionary<char, Action<Game, string>> Commands = new Dictionary<char, Action<Game, string>>()
        {
            { 'n', (Game g, string s) => { g.characters.PlayerCharacter.TryGo(Direction.North, g.maze.Rooms); } },
            { 'e', (Game g, string s) => { g.characters.PlayerCharacter.TryGo(Direction.East,g.maze.Rooms); } },
            { 's', (Game g, string s) => { g.characters.PlayerCharacter.TryGo(Direction.South, g.maze.Rooms); } },
            { 'w', (Game g, string s) => { g.characters.PlayerCharacter.TryGo(Direction.West, g.maze.Rooms); } },
            { 'l', (Game g, string s) => { Console.Write(g.maze[g.currentRoom].PrintLookAroundString()); } },
            { 't', (Game g, string s) => { g.characters.PlayerCharacter.TryTalk(s); } },
            { 'p', (Game g, string s) => { Interactions.PickupItem(g.maze, g.currentRoom, g.characters.PlayerCharacter); } },
            { 'i', (Game g, string s) => { g.characters.PlayerCharacter.PrintEquippedWeapon(); g.characters.PlayerCharacter.PrintInventory(); } },
            { 'c', (Game g, string s) => { g.characters.PlayerCharacter.PrintStats(); } },
            { 'b', (Game g, string s) => { g.interactions.Battle(g.maze,g.characters.PlayerCharacter.CurrentRoom, g.characters.PlayerCharacter); } }
        };

        public Game()
        {
            currentRoom = new Point(StartX, StartY);
            maze = new Maze(Width, Height, StartX, StartY);
            maze.Print();
            characters.AddCharacters(maze);
            ObjectDefinitions.AddItems(maze);
        }

        /// <summary>
        /// Print the room, get user input to accept commands
        /// </summary>
        public void Run()
        {
            PrintInstructions();
            while (true)
            {
                maze[currentRoom].PrintAvailableDirections();
                ProcessInput(Console.ReadLine());
            }
        }


        private void ProcessInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput.Length > 0)
            {
                Action<Game,string> action = Commands[userInput[0]];
                if (action != null)
                {
                    action(this, userInput);
                    return;
                }
            }
            PrintInstructions();
        }

        private void PrintInstructions()
        {
            Console.WriteLine("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around.");
            Console.WriteLine("[L] to look around, [P] to pick up an item, [I] Inventory, [B] Battle");
            Console.WriteLine("[C] to view stats");
        }
    }
}
