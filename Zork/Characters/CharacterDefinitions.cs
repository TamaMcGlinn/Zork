using System;
using System.Collections.Generic;
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
        
        public static void AddCharacters(Maze maze)
        {
            foreach (Character npc in NPCS)
            {
                maze.GetRandomRoom().CharactersInRoom.Add(npc);
            }
        }
    }
}
