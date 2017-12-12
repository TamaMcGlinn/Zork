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
        Player player;
        Dictionary<char, Action<Game>> Commands = new Dictionary<char, Action<Game>>()
        {
            { 'n', (Game g) => { g.player.TryGo(Direction.North, g.maze.Rooms); } },
            { 'e', (Game g) => { g.player.TryGo(Direction.East,g.maze.Rooms); } },
            { 's', (Game g) => { g.player.TryGo(Direction.South, g.maze.Rooms); } },
            { 'w', (Game g) => { g.player.TryGo(Direction.West, g.maze.Rooms); } },
            { 'l', (Game g) => { g.player.LookAround(); } },
            { 't', (Game g) => { g.player.TryTalk(); } },
            { 'p', (Game g) => { g.player.PickupItem(); } },
            { 'i', (Game g) => { g.player.PrintInventory(); } },
            { 'c', (Game g) => { g.player.PrintStats(); } },
            { 'b', (Game g) => { g.player.Battle(g.maze.Rooms); } }
        };

        public Game()
        {
            
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + Console.WindowHeight);
            maze = new Maze(Width, Height, StartX, StartY);
            player = new Player(maze[new Point(StartX, StartY)]);
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
                player.CurrentRoom.PrintAvailableDirections();
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
