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

            CharacterDefinitions characters = createPlayerCharacter();
            Player p = characters.PlayerCharacter;
            string gunName = "gun";
            string gunDescription = "description";
            p.EquippedWeapon = new Weapon(gunName, 1, gunDescription);
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            p.EquippedWeapon.PrintStats();
            Assert.AreEqual(gunName + ": " + gunDescription + 
                "\r\nStrength: " + p.EquippedWeapon.Strength + 
                "\r\n", consoleOutput.ToString());
        }

        [TestMethod]
        public void UnarmedPlayerDescriptionIsCorrect()
        {

            CharacterDefinitions characters = createPlayerCharacter();
            Player p = characters.PlayerCharacter;
            characters.PlayerCharacter.EquippedWeapon = null;
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            p.PrintStats();
            Assert.AreEqual(characters.PlayerCharacter.Name + ": " 
                + characters.PlayerCharacter.Description +
                "\r\nHealth: 100" +
                "\r\nStrength: " + characters.PlayerCharacter.Strength + 
                "\r\nUnarmed.\r\n", consoleOutput.ToString());
        }

        [TestMethod]
        public void ArmedPlayerDescriptionIsCorrect()
        {
          
            using (StringWriter consoleOutput = new StringWriter())
            {
                CharacterDefinitions characters = createPlayerCharacter();
                Player p = characters.PlayerCharacter;
                Console.SetOut(consoleOutput);
                characters.PlayerCharacter.PrintStats();
                string expectedResult = characters.PlayerCharacter.Name + ": "
                    + characters.PlayerCharacter.Description +
                    "\r\nHealth: 100" +
                    "\r\nStrength: " + characters.PlayerCharacter.Strength +
                    "\r\nCurrent weapon:" +
                    "\r\n" + p.EquippedWeapon.Name + ": " + p.EquippedWeapon.Description +
                    "\r\nStrength: " + p.EquippedWeapon.Strength + "\r\n";
                Assert.AreEqual(expectedResult, consoleOutput.ToString());
            }
        }

        public CharacterDefinitions createPlayerCharacter()
        {
            CharacterDefinitions characters = new CharacterDefinitions();
            Player p = new Player(new Zork.Room("", new System.Drawing.Point(0, 0)));
            p.EquippedWeapon = new Weapon("gun", 1, "description");
            characters.PlayerCharacter = p;
            return characters;
        }
    }
}
