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
        public const int Width = 2;
        public const int Height = 2;
        const int StartX = 1;
        const int StartY = 1;

        
        CharacterDefinitions characters = new CharacterDefinitions();
        Dictionary<char, Action<Game>> Commands = new Dictionary<char, Action<Game>>()
        {
            { 'n', (Game g) => { g.characters.PlayerCharacter.TryGo(Direction.North, g.maze.Rooms); } },
            { 'e', (Game g) => { g.characters.PlayerCharacter.TryGo(Direction.East,g.maze.Rooms); } },
            { 's', (Game g) => { g.characters.PlayerCharacter.TryGo(Direction.South, g.maze.Rooms); } },
            { 'w', (Game g) => { g.characters.PlayerCharacter.TryGo(Direction.West, g.maze.Rooms); } },
            { 'l', (Game g) => { g.characters.PlayerCharacter.LookAround(); } },
            { 't', (Game g) => { g.characters.PlayerCharacter.TryTalk(); } },
            { 'p', (Game g) => { g.characters.PlayerCharacter.PickupItem(); } },
            { 'i', (Game g) => { g.characters.PlayerCharacter.PrintInventory(); } },
            { 'c', (Game g) => { g.characters.PlayerCharacter.PrintStats(); } },
            { 'b', (Game g) => { g.characters.PlayerCharacter.Battle(g.maze.Rooms); } }
        };

        public Game()
        {
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + Console.WindowHeight);
            maze = new Maze(Width, Height, StartX, StartY);
            characters.PlayerCharacter = new Player(maze[new Point(StartX, StartY)]);
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
                maze[characters.PlayerCharacter.CurrentRoom.LocationOfRoom].PrintAvailableDirections();
                ProcessInput(Console.ReadLine());
            }
        }

        private void ProcessInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput.Length > 0)
            {
                Action<Game> action = Commands[userInput[0]];
                if (action != null)
                {
                    action(this);
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
