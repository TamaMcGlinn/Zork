using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zork.Characters
{
    public class CharacterDefinitions
    {
        public Player _player;

        public Player PlayerCharacter
        {
            get { return _player; }
            set { _player = value; }
        }
        
        private List<NPC> _npcs = new List<NPC>() {
            new NPC("alfred", "A man on horseback. He looks like he's in a hurry!", 4, 100, 2, null, false),
            new NPC("audrey", "A vile woman, penniless but with a golden smile.", 3, 60, 1, null, false),
            new NPC("sherrif_barney", "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.",3 , 100, 5, null, false),
            new NPC("henry", "A tall man with a round face, and a kingly red robe draped around his shoulders.", 12, 100, 5, new Objects.Weapon("Sword", 22, "Blackened steel sword. Looks pointy."), false),
            new NPC("lady_barclay", "Lady to Sir Barclay, of Barclay manor. She wears a black bonnet and coat.", 4, 40, 1, null, false),
            new NPC("barden", "A lad, not sixteen years of age.", 4, 40, 1, null, false),
            new NPC("kelsey", "Barclay's son, probably already betrothed to some French lady.", 8, 40, 1, null, false),
            new NPC("ignatius", "Your son. Not a blemish in his youth nor character.", 4, 40, 1, null, false),
            new NPC("bevis", "A lad, not sixteen years of age.", 4, 40, 1, null, false),
            new NPC("burton", "A lowly servant, not worth your time.", 4, 40, 1, null, true),
            new NPC("sir_barclay", "Sir Barclay; his son and yours are friends.", 14, 100, 1, null, false),
            new NPC("maxwell", "Sir Barclay's cook. He has a woman's hands, that have clearly never had to plug a leak below deck.", 8, 80, 1, null, false),
            new NPC("emerson", "Looks like an unsavory sort of fellow.", 8, 80, 1, null, false),
            new NPC("geoffrey", "By the looks of him, his brain is as dry as the remainder biscuit after voyage.", 8, 80, 1, null, false),
            new NPC("reginald", "He's no starveling. This great stinking hill of flesh has ne'er seen the inside of a shower nor the bottom of a salad bowl.", 2, 80, 1, null, false),
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
