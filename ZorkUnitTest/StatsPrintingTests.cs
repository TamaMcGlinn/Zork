using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Characters;
using Zork.Objects;

namespace ZorkUnitTest
{
    [TestClass]
    public class StatsPrintingTests
    {
        [TestMethod]
        public void WeaponDescriptionIsCorrect()
        {
            Player p = new Player();
            p.EquippedWeapon = new Weapon("gun", 1, "description");
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            p.EquippedWeapon.PrintStats();
            Assert.AreEqual("gun: description\nStrength: 1\n", consoleOutput.ToString());
        }
    }
}
