using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zork.Characters
{
    public static class CharacterDefinitions
    {
        private static Player _player = new Player();

        public static Player PlayerCharacter
        {
            get { return _player; }
        }

        private static List<NPC> _npcs = new List<NPC>() {
            new NPC("sherrif_barney", "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.", 3, 100),
            new NPC("henry", "A tall man with a round face, and a kingly red robe draped around his shoulders.", 12, 100, new Objects.Weapon("Sword", 22, "Blackened steel sword. Looks pointy."))
        };

        public static List<NPC> NPCS
        {
            get { return _npcs; }
        }

        private static Dictionary<NPC, Point> _characterLocations = new Dictionary<NPC, Point>();

        public static void AddCharacters(Maze maze)
        {
            foreach (NPC npc in NPCS)
            {
                Point location = maze.GetRandomRoom();
                maze[location].NPCsInRoom.Add(npc);
                _characterLocations.Add(npc, location);
            }
        }

        /// <summary>
        /// Move each NPC if it is time to move it.
        /// </summary>
        public static void MoveNPCs(Maze maze)
        {
            foreach(KeyValuePair<NPC,Point> kvp in _characterLocations)
            {
                NPC npc = kvp.Key;
                Point location = kvp.Value;
                if (npc.IsTimeToMove())
                {
                    MoveNPC(maze, location, npc);
                    npc.PickNextTimeToMove();
                    npc.PickNextTimeToMove();
                }
                npc.LowerTurnsToNextMove();
            }
        }

        private static void MoveNPC(Maze maze, Point currentLocation, NPC npc)
        {
            var rng = new Random();
            var options = maze.AccessibleNeighbours(currentLocation).ToList();
            var newRoom = options[rng.Next(0,options.Count)];
            maze[currentLocation].NPCsInRoom.Remove(npc);
            maze[newRoom].NPCsInRoom.Add(npc);
        }
    }
}
