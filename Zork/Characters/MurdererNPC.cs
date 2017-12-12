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
        int roundsBeforeNewKill = 10;
        public MurdererNPC(string name, string description, int strength, int startHealth, int letsPlayerFleePerXRounds, Weapon weapon = null, bool isMurderer = false) : base(name, description, strength, startHealth, letsPlayerFleePerXRounds, weapon)
        {
        }

        public MurdererNPC(string name, string description, int strength, int startHealth, int maxHealth, int letsPlayerFleePerXRounds, Weapon weapon = null, bool isMurderer = false) : base(name, description, strength, startHealth, maxHealth, letsPlayerFleePerXRounds, weapon)
        {
        }

        public override void Turn()
        {
            if (roundsBeforeNewKill == 0)
            {
                KillNPCInSameRoom();
            }
            roundsBeforeNewKill--;
            base.Turn();
           
        }

        public void KillNPCInSameRoom()
        {
            Random rng = new Random();
            int killNpc = rng.Next(0, CurrentRoom.NPCsInRoom.Count - 1);
            List<NPC> npcsInCurrentRoom = 
            CurrentRoom.NPCsInRoom

        }
    }
}
