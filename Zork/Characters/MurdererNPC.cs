using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;

namespace Zork.Characters
{
    public class MurdererNPC : NPC
    {
        public int killEveryXRounds { get; set; } = 10;
        public int roundsBeforeNewKill;

        public MurdererNPC(string name, string description, int strength, int startHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, letsPlayerFleePerXRounds, weapon)
        {
            roundsBeforeNewKill = killEveryXRounds;
        }

        public MurdererNPC(string name, string description, int strength, int startHealth, int maxHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, letsPlayerFleePerXRounds, weapon)
        {
            roundsBeforeNewKill = killEveryXRounds;
        }

        public override void Turn()
        {
            roundsBeforeNewKill--;
            if (roundsBeforeNewKill == 0)
            {
                KillRandomNPCInSameRoom();
                roundsBeforeNewKill = killEveryXRounds;
            }
            base.Turn();
           
        }

        /// <summary>
        /// Kills someone in the current room
        /// </summary>
        /// <returns>true if the murderer killed someone, false if he didnt</returns>
        public bool KillRandomNPCInSameRoom()
        {
            if (CurrentRoom.NPCsInRoom.Count > 0)
            {
                Random rng = new Random();

                int killNpc = rng.Next(0, CurrentRoom.NPCsInRoom.Count - 1);
                List<NPC> npcsInCurrentRoom = CurrentRoom.NPCsInRoom;
                npcsInCurrentRoom[killNpc].DropWeapon();
                npcsInCurrentRoom[killNpc].DropAllItems();
                npcsInCurrentRoom.RemoveAt(killNpc);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
