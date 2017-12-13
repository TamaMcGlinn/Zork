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
        private Maze maze;
        public const int Width = 2;
        public const int Height = 2;
        const int StartX = 1;
        const int StartY = 1;
        public Player player;

        public List<NPC> NPCS = new List<NPC>() {
            new NPC("sherrif_barney", "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.",3 , 100, 5, null, false),
            new NPC("henry", "A tall man with a round face, and a kingly red robe draped around his shoulders.", 12, 100, 5, new Objects.Weapon("Sword", 22, "Blackened steel sword. Looks pointy."), true)
        };

        private Dictionary<char, Action<Game>> commands = new Dictionary<char, Action<Game>>()
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
            { 'b', (Game g) => { g.player.Battle(g.maze); } }
        };

        public Game()
        {
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + Console.WindowHeight);
            maze = new Maze(Width, Height, StartX, StartY);
            player = new Player(maze[new Point(StartX, StartY)]);
            AddCharacters();
            ObjectDefinitions.AddItems(maze);
        }

        private void AddCharacters()
        {
            foreach (NPC npc in NPCS)
            {
                Point location = maze.GetRandomRoom();
                Room room = maze[location];
                npc.maze = maze;
                room.NPCsInRoom.Add(npc);
                npc.CurrentRoom = room;
                player.Moved += npc.OnPlayerMoved;
            }
        }

        /// <summary>
        /// Print the room, get user input to accept commands
        /// </summary>
        public void Run()
        {
            maze.Print();
            PrintInstructions();
            while (true)
            {
                string roomDescription = player.CurrentRoom.DescribeAvailableDirections();
                Console.WriteLine(roomDescription);
                ProcessInput(Console.ReadLine());
            }
        }

        private void ProcessInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput.Length > 0)
            {
                Action<Game> action = commands[userInput[0]];
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
