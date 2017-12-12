using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork;
using Zork.Characters;

namespace ZorkUnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class FleeBehaviourTests
    {
        [TestMethod]
        public void FleeFunctionalityTest()
        {
            Maze m = new Maze(Game.Width, Game.Height, 0, 0);

            CharacterDefinitions cd = new CharacterDefinitions();
            Player p = cd.PlayerCharacter;
            p.CurrentRoom = new Room("", new Point(0, 0));
            int oldX = p.CurrentRoom.LocationOfRoom.X;
            int oldY = p.CurrentRoom.LocationOfRoom.Y;
            p.Flee(m.Rooms);
            Assert.IsTrue(p.CurrentRoom.LocationOfRoom.X != oldX || p.CurrentRoom.LocationOfRoom.Y != oldY);
        }
    }
}
