using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Characters;
using Zork.Extensions;

namespace ZorkUnitTest
{
    [TestClass]
    public class ListExtensionTest
    {
        [TestMethod]
        public void TestFindNPCEmpty()
        {
            List<NPC> a = new List<NPC>();
            Assert.IsNull(a.FindNPC("f"));
        }

        [TestMethod]
        public void TestFindNPCNonEmpty()
        {
            List<NPC> a = new List<NPC>() { new NPC("", "", 0, 0, 0, null) };
            Assert.IsNotNull(a.FindNPC(""));
        }
    }
}
