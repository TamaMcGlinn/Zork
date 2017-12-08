using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            new NPC("sherrif_barney", "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.", 3, 100)
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

        public static void MoveNPCs(Maze maze)
        {
            foreach(var kvp in _characterLocations)
            {
                if (kvp.Key.IsTimeToMove())
                {
                    MoveNPC(maze, kvp.Value, kvp.Key);
                    kvp.Key.PickNextTimeToMove();
                    kvp.Key.PickNextTimeToMove();
                }
                kvp.Key.PlayerMoved();
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
