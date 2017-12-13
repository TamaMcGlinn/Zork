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
        public int killEveryXPlayerSteps { get; set; } = 10;
        public int stepsBeforeNewKill;

        public MurdererNPC(string name, string description, int strength, int startHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, letsPlayerFleePerXRounds, weapon)
        {
            stepsBeforeNewKill = killEveryXPlayerSteps;
        }

        public MurdererNPC(string name, string description, int strength, int startHealth, int maxHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, letsPlayerFleePerXRounds, weapon)
        {
            stepsBeforeNewKill = killEveryXPlayerSteps;
        }

        public void WalkingTurn(Maze m)
        {
            if (IsTimeToMove())
            {
                MoveNPCToRandomSurroundingRoom(m);
            }
            if (stepsBeforeNewKill == 0)
            {
                KillRandomNPCInSameRoom();
                stepsBeforeNewKill = killEveryXPlayerSteps;
            }
            LowerTurnsToNextMove();
            stepsBeforeNewKill--;
           
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
