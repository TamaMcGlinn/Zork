using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    class Room
    {
        private string description;

        public Room(string desc)
        {
            description = desc;
        }

        public void Print() {
            Console.WriteLine(description);
        }
    }
}
