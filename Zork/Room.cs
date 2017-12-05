using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    class Room
    {
        private List<Character> _characters;

        public List<Character> Characters
        {
            get { return _characters; }
        }

        private string description;

        public Room(string desc)
        {
            description = desc;
            _characters = new List<Character>();
        }

        public void Print() {
            Console.WriteLine(description);
            Console.WriteLine("Characters here:");
            foreach(Character c in Characters)
            {
                string formattedName = c.Name.Replace('_', ' ');
                Console.WriteLine(formattedName);
            }
        }
    }
}
