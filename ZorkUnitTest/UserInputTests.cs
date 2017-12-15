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
    }
}
