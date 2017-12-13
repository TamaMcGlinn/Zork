using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zork;
using System.Drawing;
using System.Collections.Generic;

namespace ZorkUnitTest
{
    [TestClass]
    public class TestMaze
    {
        [TestMethod]
        public void HomeExists()
        {
            Maze maze = new Maze(5, 5, 0, 0);
            bool foundHouse = false;
            foreach (Room room in maze)
            {
                if (room.Description == Maze.HouseDescription)
                {
                    foundHouse = true;
                }
            }
            Assert.IsTrue(foundHouse);
        }

        [TestMethod]
        public void StreetNamesAreUnique()
        {
            Maze maze = new Maze(5, 5, 0, 0);
            HashSet<string> descriptions = new HashSet<string>();
            foreach (Room room in maze)
            {
                Assert.IsFalse(descriptions.Contains(room.Description));
                descriptions.Add(room.Description);
            }
        }

        [TestMethod]
        public void CreateRoom()
        {
            Maze m = new Maze(5, 5, 0, 0);
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void CantExitAtSides()
        {
            Maze m = new Maze(5, 5, 0, 0);
            for(int xi = 0; xi < 5; ++xi)
            {
                Assert.AreEqual(false, m[xi, 0].CanGoThere[Direction.North]);
                Assert.AreEqual(false, m[xi, 4].CanGoThere[Direction.South]);
            }
            for (int yi = 0; yi < 5; ++yi)
            {
                Assert.AreEqual(false, m[0, yi].CanGoThere[Direction.West]);
                Assert.AreEqual(false, m[4, yi].CanGoThere[Direction.East]);
            }
        }

        /// <summary>
        /// Checks that all rooms are accessible from Point(0,0)
        /// </summary>
        [TestMethod]
        public void AllRoomsAccessibleFromOrigin()
        {
            const int Width = 5;
            const int Height = 5;
            Maze m = new Maze(Width, Height, 0, 0);
            HashSet<Point> accessibleRooms = new HashSet<Point>();
            // the rooms we've visited but which have not yet had their CanGoThere dictionary checked
            Queue<Point> endPoints = new Queue<Point>();
            endPoints.Enqueue( new Point(0,0) );
            while (endPoints.Count > 0) {
                Point currentRoom = endPoints.Dequeue();
                accessibleRooms.Add(currentRoom);
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    Point neighbour = currentRoom.Add(dir);
                    if (m[currentRoom].CanGoThere[dir] && !accessibleRooms.Contains(neighbour))
                    {
                        endPoints.Enqueue(neighbour); // enqueue all new reachable rooms, to be added in later iterations
                    }
                }
            }
            Assert.AreEqual(accessibleRooms.Count, Width * Height);
        }
    }
}
