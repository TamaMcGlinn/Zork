using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;

namespace ZorkUnitTest
{
    [TestClass]
    public class MapPrinterTest
    {
        [TestMethod]
        public void PlayerIsSomeWhere()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Maze maze = new Maze(10, 10, 1, 1);
                new MapPrinter(maze).Print(new Point(0, 0), new List<Point>());
                Assert.IsTrue(sw.ToString().Contains(MapPrinter.PlayerSymbol));
            }
        }
    }
}
