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
        public int KillEveryXPlayerSteps { get; set; } = 20;
        public int StepsBeforeNextKill { get; set; }

        public MurdererNPC(string name, string description, int strength, int startHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, letsPlayerFleePerXRounds, weapon)
        {
            StepsBeforeNextKill = KillEveryXPlayerSteps;
        }

        public MurdererNPC(string name, string description, int strength, int startHealth, int maxHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, letsPlayerFleePerXRounds, weapon)
        {
            StepsBeforeNextKill = KillEveryXPlayerSteps;
        }
        
        public override void OnPlayerMoved()
        {
            base.OnPlayerMoved();
            StepsBeforeNextKill++;
            if(StepsBeforeNextKill == KillEveryXPlayerSteps)
            {
                KillRandomNPCInSameRoom();
            }
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

                int killNpc = rng.Next(0, CurrentRoom.NPCsInRoom.Count);
                List<NPC> npcsInCurrentRoom = CurrentRoom.NPCsInRoom;
                if (npcsInCurrentRoom[killNpc] != this)
                {
                    npcsInCurrentRoom[killNpc].DropWeapon();
                    npcsInCurrentRoom[killNpc].DropAllItems();
                    npcsInCurrentRoom.RemoveAt(killNpc);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
