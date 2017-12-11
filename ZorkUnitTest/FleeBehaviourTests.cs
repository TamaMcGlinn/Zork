using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;

namespace ZorkUnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class FleeBehaviourTests
    {
        [TestMethod]
        public void FleeFunctionalityTest()
        {
            FleeBehaviour fleeBehaviour = new FleeBehaviour(5);
            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, 0);
            fleeBehaviour.Flee(p1);
            Assert.IsTrue(p1.X != p2.X && p1.Y != p2.Y);
        }
    }
}
