using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    public class Room
    {
        private string description;
        public Dictionary<Direction,bool> canGoThere;

        public Room(string desc)
        {
            canGoThere = new Dictionary<Direction, bool> {
                { Direction.North, false },
                { Direction.East, false },
                { Direction.South, false },
                { Direction.West, false }
            };
            description = desc;
        }

        public void Print() {
            Console.WriteLine(description);
            Console.WriteLine("You can go:");
            foreach(var kvp in canGoThere)
            {
                if( kvp.Value)
                {
                    Console.WriteLine(kvp.Key);
                }
            }
        }
    }
}
