using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zork.Characters
{
    public class CharacterDefinitions
    {
        private Player _player = new Player(new Room("", new System.Drawing.Point(0,0)));

        public Player PlayerCharacter
        {
            get { return _player; }
        }
        
        private List<NPC> _npcs = new List<NPC>() {
            new NPC("sherrif_barney", "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.",3 , 100, 5, null, false),
            new NPC("henry", "A tall man with a round face, and a kingly red robe draped around his shoulders.", 12, 100, 5, new Objects.Weapon("Sword", 22, "Blackened steel sword. Looks pointy."), true)
        };

        public List<NPC> NPCS
        {
            get { return _npcs; }
        }

        public void AddCharacters(Maze maze)
        {
            foreach (NPC npc in NPCS)
            {
                Point location = maze.GetRandomRoom();
                Room room = maze[location];
                room.NPCsInRoom.Add(npc);
                npc.CurrentRoom = room;
            }
        }

        /// <summary>
        /// Move each NPC if it is time to move it.
        /// </summary>
        public void MoveNPCs(Maze maze)
        {
            foreach(NPC npc in NPCS)
            {
                Point location = npc.CurrentRoom.LocationOfRoom;
                if (npc.IsTimeToMove())
                {
                    MoveNPC(maze, location, npc);
                    npc.PickNextTimeToMove();
                }
                npc.LowerTurnsToNextMove();
            }
        }

        private void MoveNPC(Maze maze, Point currentLocation, NPC npc)
        {
            var rng = new Random();
            var options = maze.AccessibleNeighbours(currentLocation).ToList();
            var newRoom = options[rng.Next(0,options.Count)];
            maze[currentLocation].NPCsInRoom.Remove(npc);
            maze[newRoom].NPCsInRoom.Add(npc);
        }
    }
}
