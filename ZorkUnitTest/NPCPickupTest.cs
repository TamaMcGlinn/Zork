using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;
using Zork.Characters;

namespace ZorkUnitTest
{
    [TestClass]
    public class NPCPickupTest
    {
        [TestMethod]
        public void NPCPicksUpWeapon()
        {
            using (ShimsContext.Create())
            {
                Zork.Fakes.ShimChance.PercentageInt32 = (_) =>
                {
                    return true;
                };
                Game game = new Game();
                foreach (Room room in game.maze)
                {
                    room.ObjectsInRoom.Add(CharacterTests.CreateWeapon());
                }
                NPC npc = game.NPCS.First();
                for (int i = 0; i < NPC.MaxTurnsBetweenMoves; ++i)
                {
                    npc.OnPlayerMoved(game);
                }
                Assert.IsTrue(npc.Inventory.Count >= 1);
            }
        }
    }
}
