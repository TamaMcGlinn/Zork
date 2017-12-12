using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;

namespace Zork.Characters
{
    class MurdererNPC : NPC
    {
        public MurdererNPC(string name, string description, int strength, int startHealth, int letsPlayerFleePerXRounds, Weapon weapon = null, bool isMurderer = false) : base(name, description, strength, startHealth, letsPlayerFleePerXRounds, weapon)
        {
        }

        public MurdererNPC(string name, string description, int strength, int startHealth, int maxHealth, int letsPlayerFleePerXRounds, Weapon weapon = null, bool isMurderer = false) : base(name, description, strength, startHealth, maxHealth, letsPlayerFleePerXRounds, weapon)
        {
        }
    }
}
