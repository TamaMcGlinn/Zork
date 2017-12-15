using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;

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
            using (var sr = new StringReader("t\n1\nq"))
            {
                Console.SetIn(sr);
                Program.Main(new string[0]);
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
