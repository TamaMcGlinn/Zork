using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using Zork.Objects;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

namespace ZorkUnitTest
{
    [TestClass]
    public class ObjectDefinitionsUnitTests
    {
        [TestMethod]
        public void AddingHealthPickupsTest()
        {
            int sizex = 5;
            int sizey = 5;
            Maze m = new Maze(5,5,0,0);
            ObjectDefinitions.AddItems(m);
            List<BaseObject> objects = new List<BaseObject>();
            
            for (int x = 0; x < sizex; x++)
            {
                for (int y = 0; y < sizey; y++)
                {
                    objects.AddRange(m[new Point(x, y)].ObjectsInRoom);
                }
            }
            Assert.IsTrue(objects.Any(b => b is HealthPickup));
        }

        [TestMethod]
        public void AddingCluesPickupsTest()
        {
            int sizex = 5;
            int sizey = 5;
            Maze m = new Maze(5, 5, 0, 0);
            ObjectDefinitions.AddItems(m);
            List<BaseObject> objects = new List<BaseObject>();

            for (int x = 0; x < sizex; x++)
            {
                for (int y = 0; y < sizey; y++)
                {
                    objects.AddRange(m[new Point(x, y)].ObjectsInRoom);
                }
            }
            Clue clue = null;
            foreach (var item in objects)
            {
                if (item is Clue)
                {
                    clue = item as Clue;
                    break;
                }
            }
            Assert.IsTrue(clue != null);

        }

        [TestMethod]
        public void AddingWeaponsTest()
        {
            int sizex = 5;
            int sizey = 5;
            Maze m = new Maze(5, 5, 0, 0);
            ObjectDefinitions.AddItems(m);
            List<BaseObject> objects = new List<BaseObject>();

            for (int x = 0; x < sizex; x++)
            {
                for (int y = 0; y < sizey; y++)
                {
                    objects.AddRange(m[new Point(x, y)].ObjectsInRoom);
                }
            }
            Assert.IsTrue(objects.Any(b => b is Weapon));
        }

        /// <summary>
        /// tests if a a weapon is dropped if the dropchance is 100 percent
        /// </summary>
        [TestMethod]
        public void DropChanceTest()
        {
            Assert.IsTrue(Chance.Percentage(100));
        }

        [TestMethod]
        public void TestCreateCorpseObject()
        {
            CorpseNPCObject corpse = new CorpseNPCObject("", "desc");
            Assert.IsNotNull(corpse);

        }

        [TestMethod]
        public void TestCreateCorpsePickup()
        {
            CorpseNPCObject corpse = new CorpseNPCObject("", "desc");
            string output = "";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                corpse.PickupObject(null);
                output = sw.ToString();
                
            }
            Assert.IsTrue(output.Contains("Can't pickup a dead body"));
        }
    }
}
