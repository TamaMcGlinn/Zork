using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;
using Zork.Characters;
using Zork.Objects;

namespace ZorkUnitTest
{
    [TestClass]
    public class UserInputTests
    {
        [TestMethod]
        public void TestExitCommand()
        {
            using (var sr = new StringReader("q"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestWalkCommand()
        {
            using (var sr = new StringReader("w\nn\ne\ns\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestLookaroundCommand()
        {
            using (var sr = new StringReader("l\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestStatsCommand()
        {
            using (var sr = new StringReader("c\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestTalkCommand()
        {
            using (var sr = new StringReader("t\n1\n1\n1\n1\n1\n1\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestTryPickUp()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (var sr = new StringReader("p\n1\ni\nq"))
                {
                    Console.SetOut(stringWriter);
                    Game game = new Game();
                    game.player.CurrentRoom.ObjectsInRoom.Clear();
                    game.player.CurrentRoom.ObjectsInRoom.Add(new HealthPickup("healthPickup", 10, "a healthy pickup"));
                    Console.SetIn(sr);
                    game.Run();
                    string consoleOutput = stringWriter.ToString();
                    Assert.IsTrue(consoleOutput.Contains("You currently have the following items:\r\nhealthPickup a healthy pickup"));
                }
            }
        }

        [TestMethod]
        public void TestUseObject()
        {
            using (var sr = new StringReader("1\nq"))
            {
                Game game = new Game();
                game.player.Inventory.Clear();
                game.player.Inventory.Add(new HealthPickup("healthPickup", 10, "a healthy pickup"));
                game.player.Health = 90;
                Console.SetIn(sr);
                game.player.UseObject();
                Assert.IsTrue(game.player.Health==100);
            }
        }

        [TestMethod]
        public void TestBattleEnemy()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (var sr = new StringReader("b\n1\nq"))
                {
                    Console.SetOut(stringWriter);
                    Game game = new Game();
                    game.player.CurrentRoom.NPCsInRoom.Clear();
                    NPC npc = CharacterTests.CreateNPC();
                    npc.CurrentRoom = game.player.CurrentRoom;
                    game.player.CurrentRoom.NPCsInRoom.Add(npc);
                    Console.SetIn(sr);
                    game.Run();
                    string output = stringWriter.ToString();
                    Assert.IsTrue(output.Contains("You hit for"));
                }
            }
        }

        [TestMethod]
        public void TestTalkToPlayer()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (var sr = new StringReader("t\n1\n1\n1\n1\n1\n1\nq"))
                {

                    Console.SetOut(stringWriter);
                    Game game = new Game();
                    game.player.CurrentRoom.NPCsInRoom.Add(game.NPCS.Find(npc => npc.Name == "sir_barclay"));
                    Console.SetIn(sr);
                    game.Run();
                    string consoleOutput = stringWriter.ToString();
                    Assert.IsTrue(consoleOutput.Contains("Good day. I'm looking for clues as to the murder of one Cecil, a lady of unsavoury profession."));
                }
            }
        }


        [TestMethod]
        public void TestInventoryCommand()
        {
            using (var sr = new StringReader("i\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestPickupCommand()
        {
            using (var sr = new StringReader("p\n1\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestBattleCommand()
        {
            using (var sr = new StringReader("b\n1\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestMapCommand()
        {
            using (var sr = new StringReader("m\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }

        [TestMethod]
        public void TestuseCommand()
        {
            using (var sr = new StringReader("u\n1\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
            }
        }
    }
}
