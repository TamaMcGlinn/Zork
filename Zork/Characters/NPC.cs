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
        public NPC(string name, string description, int strength, int startHealth, Weapon weapon = null) : this(name, description, strength, startHealth, startHealth, weapon)
        {
        }

        public NPC(string name, string description, int strength, int startHealth, int maxHealth, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, weapon)
        {
        }
    }
}
