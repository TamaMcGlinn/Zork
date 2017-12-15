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
        
        public void PossiblyKillSomeone(Game game)
        {
            if (StepsBeforeNextKill > 0)
            {
                StepsBeforeNextKill--;
            }
            if (StepsBeforeNextKill == 0)
            {
                KillRandomNPCInSameRoom(game);
            }
        }

        /// <summary>
        /// Kills someone in the current room
        /// </summary>
        /// <returns>true if the murderer killed someone, false if he didnt</returns>
        public void KillRandomNPCInSameRoom(Game game)
        {
            List<NPC> otherNPCs = CurrentRoom.NPCsInRoom.Where(x => x != this).ToList();
            if (otherNPCs.Count > 0)
            {
                NPC victim = Chance.RandomElement(otherNPCs);
                victim.KillThisNPC(game);
                StepsBeforeNextKill = KillEveryXPlayerSteps;
            }
        }
    }
}
