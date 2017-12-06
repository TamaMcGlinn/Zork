using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;

namespace Zork.Characters
{
    public class NPC : Character
    {
        
        public NPC(string name, string description) : base(name, description)
        {
        }

        public NPC(string name, int strength, int health, Weapon weapon, string description) : base(name, strength, health, weapon, description)
        {
        }
    }
}
