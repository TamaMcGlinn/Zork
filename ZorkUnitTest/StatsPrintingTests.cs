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
            Player p = CharacterTests.CreatePlayerCharacter();
            string gunName = "gun";
            string gunDescription = "description";
            p.EquippedWeapon = new Weapon(gunName, 1, gunDescription);
            using (StringWriter consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                p.EquippedWeapon.PrintStats();
                Assert.AreEqual(gunName + ": " + gunDescription +
                    "\r\nStrength: " + p.EquippedWeapon.Strength +
                    "\r\n", consoleOutput.ToString());
            }
        }

        [TestMethod]
        public void UnarmedPlayerDescriptionIsCorrect()
        {
            Player p = CharacterTests.CreatePlayerCharacter();
            p.EquippedWeapon = null;
            using (StringWriter consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                p.PrintStats();
                Assert.AreEqual(p.Name + ": "
                    + p.Description +
                    "\r\nHealth: 100" +
                    "\r\nStrength: " + p.Strength +
                    "\r\nUnarmed.\r\n\r\n", consoleOutput.ToString());
            }
        }

        [TestMethod]
        public void ArmedPlayerDescriptionIsCorrect()
        {
          
            using (StringWriter consoleOutput = new StringWriter())
            {
                Player p = CharacterTests.CreatePlayerCharacter();
                Console.SetOut(consoleOutput);
                p.PrintStats();
                string expectedResult = p.Name + ": "
                    + p.Description +
                    "\r\nHealth: 100" +
                    "\r\nStrength: " + p.Strength +
                    "\r\nCurrent weapon:" +
                    "\r\n" + p.EquippedWeapon.Name + ": " + p.EquippedWeapon.Description +
                    "\r\nStrength: " + p.EquippedWeapon.Strength + "\r\n\r\n";
                Assert.AreEqual(expectedResult, consoleOutput.ToString());
            }
        }
    }
}
