using System.Collections.Generic;

namespace Zork.Characters
{
    public class CharacterDefinitions
    {
        private Player _player = new Player();

        public Player PlayerCharacter
        {
            get { return _player; }
        }

        private List<NPC> _npcs = new List<NPC>() {
            new NPC("sherrif_barney", 3, 100, null, "A fat man in a prim black sherrif's uniform. He has a mustache and short brown hair.")
        };

        public List<NPC> NPCS
        {
            get { return _npcs; }
        }
        
        public void AddCharacters(Maze maze)
        {
            foreach (Character npc in NPCS)
            {
                maze.GetRandomRoom().CharactersInRoom.Add(npc);
            }
        }
    }
}
