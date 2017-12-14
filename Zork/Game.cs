using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Zork.Characters;
using Zork.Objects;
using Zork.UIContext;

namespace Zork
{
    /// <summary>
    /// A Zork Game, which initialises the rooms, characters and objects, and allows you to play.
    /// </summary>
    public class Game
    {
        public Maze maze;
        public const int Width = 30;
        public const int Height = 18;
        const int StartX = 1;
        const int StartY = 1;
        public Player player;

        public List<NPC> NPCS = new List<NPC>() {
            new NPC("alfred", "A man on horseback. He looks like he's in a hurry!", 4, 100, 2, null ),
            new NPC("audrey", "A vile woman, penniless but with a golden smile.", 3, 60, 1, null ),
            new NPC("constable_barney", "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.",3 , 100, 5, null ),
            new NPC("henry", "A tall man with a round face, and a kingly red robe draped around his shoulders.", 12, 100, 5, new Objects.Weapon("Sword", 22, "Blackened steel sword. Looks pointy.") ),
            new NPC("lady_barclay", "Lady to Sir Barclay, of Barclay manor. She wears a black bonnet and coat.", 4, 40, 1, null ),
            new NPC("barden", "A lad, not sixteen years of age.", 4, 40, 1, null ),
            new NPC("kelsey", "Barclay's son, probably already betrothed to some French lady.", 8, 40, 1, null ),
            new NPC("ignatius", "Your son. Not a blemish in his youth nor character.", 4, 40, 1, null ),
            new NPC("bevis", "A lad, not sixteen years of age.", 4, 40, 1, null ),
            new MurdererNPC("burton", "A lowly servant, not worth your time.", 4, 40, 1, null),
            new NPC("sir_barclay", "Sir Barclay; his son and yours are friends.", 14, 100, 1, null ),
            new NPC("maxwell", "Sir Barclay's cook. He has a woman's hands, that have clearly never had to plug a leak below deck.", 8, 80, 1, null ),
            new NPC("emerson", "Looks like an unsavory sort of fellow.", 8, 80, 1, null ),
            new NPC("geoffrey", "By the looks of him, his brain is as dry as the remainder biscuit after voyage.", 8, 80, 1, null ),
            new NPC("reginald", "He's no starveling. This great stinking hill of flesh has ne'er seen the inside of a shower nor the bottom of a salad bowl.", 2, 80, 1, null )
        };

        private Dictionary<char, Action<Game>> commands = new Dictionary<char, Action<Game>>()
        {
            { 'n', (Game g) => { g.player.TryGo(Direction.North, g); } },
            { 'e', (Game g) => { g.player.TryGo(Direction.East,g); } },
            { 's', (Game g) => { g.player.TryGo(Direction.South, g); } },
            { 'w', (Game g) => { g.player.TryGo(Direction.West, g); } },
            { 'l', (Game g) => { g.player.LookAround(); } },
            { 't', (Game g) => { g.player.TryTalk(); } },
            { 'p', (Game g) => { g.player.PickupItem(); } },
            { 'i', (Game g) => { g.player.PrintInventory(); } },
            { 'c', (Game g) => { g.player.PrintStats(); } },
            { 'b', (Game g) => { g.player.Battle(g); } },
            { 'm', (Game g) => { g.maze.Print(g.player.CurrentRoom.LocationOfRoom, g.GetNPCLocations()); } },
            { 'u', (Game g) => { g.player.UseObject(); }}
        };

        private List<Point> GetNPCLocations()
        {
            return NPCS.ConvertAll((NPC npc) => { return npc.CurrentRoom.LocationOfRoom; }).ToList();
        }

        public Game()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
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
            }
        }

        /// <summary>
        /// Print the room, get user input to accept commands
        /// </summary>
        public void Run()
        {
            PrintPreamble();
            PrintInstructions();
            while (true)
            {
                player.CurrentRoom.PrintRoom();
                ProcessInput(Console.ReadLine());
                Console.WriteLine();
            }
        }

        private void PrintPreamble()
        {
            using (new ColorContext(ColorContext.PreambleColor))
            {
                Console.WriteLine("You are Sherlock, a reknowned detective. In ye olde London, a most vile place to be,");
                Console.WriteLine("thismorning dead was found dear Cecil, dear to many men. Serve justice to the murderer!\n");
            }
        }

        private void ProcessInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput.Length > 0)
            {
                if (commands.ContainsKey(userInput[0]))
                {
                    commands[userInput[0]](this);
                    return;
                }
            }
            PrintInstructions();
        }

        private void PrintInstructions()
        {
            using (new ColorContext(ColorContext.InstructionsColor))
            {
                ColorContext.PrintWithKeyCodes("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around,\n");
                ColorContext.PrintWithKeyCodes("[L] to look around, [P] to pick up an item, [I] for Inventory, [B] for Battle,\n");
                ColorContext.PrintWithKeyCodes("[C] to view stats, or [M] to print the map. [U] to use items.\n\n");
            }
        }
    }
}
