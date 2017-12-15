using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Zork.Objects;
using Zork.Properties;
using Zork.UIContext;

namespace Zork
{
    public class Maze : IEnumerable<Room>
    {
        public Room[,] Rooms { get; set; }
        public const string HouseDescription = "Your house";
        public readonly int Width;
        public readonly int Height;
        private List<string> streetNames = new List<string>();

        public Maze(int xSize, int ySize, int StartX, int StartY)
        {
            Width = xSize;
            Height = ySize;
            Rooms = new Room[xSize, ySize];
            Rooms[StartX, StartY] = new Room(HouseDescription, new Point(StartX, StartY));

            string streetnames = Resources.streetnames;
            
            streetNames.AddRange(streetnames.Split(new string[] { "\r\n"}, StringSplitOptions.None ));
            CreateNeighbour(new Point(StartX, StartY));
            AddExtraConnections(xSize * ySize);
        }

        public Room this[int x, int y]
        {
            get { return Rooms[x, y]; }
        }

        public Room this[Point p]
        {
            get { return this[p.X, p.Y]; }
        }

        public void AddItemToRandomRoom(BaseObject obj)
        {
            this[GetRandomRoom()].ObjectsInRoom.Add(obj);
        }

        public Point GetRandomRoom()
        {
            return new Point(Chance.Between(0, Width), Chance.Between(0, Height));
        }

        private void AddExtraConnections(int extras)
        {
            if(Width < 3 || Height < 3)
            {
                return;
            }
            for (int i = 0; i < extras; ++i)
            {
                var roomToConnect = new Point(Chance.Between(1, Width - 1), Chance.Between(1, Height - 1));
                var neighbourPoint = GetRandomNeighbour(roomToConnect);
                Connect(roomToConnect, neighbourPoint);
            }
        }

        public Point GetRandomNeighbour(Point roomToConnect)
        {
            Direction[] allDirections = (Direction[])Enum.GetValues(typeof(Direction));
            Direction direction = Chance.RandomElement(allDirections);
            return roomToConnect.Add(direction);
        }

        public Room GetRandomOtherRoom(Room currentRoom)
        {
            List<Room> otherRooms = new List<Room>();
            foreach(Room room in this)
            {
                if (room.LocationOfRoom != currentRoom.LocationOfRoom)
                {
                    otherRooms.Add(room);
                }
            }
            if(otherRooms.Count == 0)
            {
                return null;
            }
            return Chance.RandomElement(otherRooms);
        }

        /// <summary>
        /// Set the canGoThere variable to true between the two locations.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void Connect(Point a, Point b)
        {
            if (a.X == b.X)
            {
                ConnectVertical(a, b);
            }
            else
            {
                ConnectHorizontal(a, b);
            }
        }

        public void Print(Point playerLocation, List<Point> npcLocations)
        {
            new MapPrinter(this).Print(playerLocation, npcLocations);
        }

        private void ConnectHorizontal(Point a, Point b)
        {
            if (a.X + 1 == b.X)
            {
                Rooms[a.X, a.Y].CanGoThere[Direction.East] = true;
                Rooms[b.X, b.Y].CanGoThere[Direction.West] = true;
            }
            else
            {
                Debug.Assert(a.X - 1 == b.X);
                Rooms[a.X, a.Y].CanGoThere[Direction.West] = true;
                Rooms[b.X, b.Y].CanGoThere[Direction.East] = true;
            }
        }

        private void ConnectVertical(Point a, Point b)
        {
            if (a.Y + 1 == b.Y)
            {
                Rooms[a.X, a.Y].CanGoThere[Direction.South] = true;
                Rooms[b.X, b.Y].CanGoThere[Direction.North] = true;
            }
            else
            {
                Debug.Assert(a.Y - 1 == b.Y);
                Rooms[a.X, a.Y].CanGoThere[Direction.North] = true;
                Rooms[b.X, b.Y].CanGoThere[Direction.South] = true;
            }
        }

        /// <summary>
        /// Return the neighbouring points for which the room is null.
        /// </summary>
        /// <param name="place">The target location to examine</param>
        /// <returns></returns>
        private List<Point> ListEmptyNeighbours(Point place)
        {
            return place.ListNeighbours(this).Where((Point neighbour) => {
                return this[neighbour] == null;
            }).ToList();
        }

        /// <summary>
        /// Return the neighbouring points accessible from the place.
        /// </summary>
        /// <param name="place">The target location to examine</param>
        /// <returns></returns>
        public IEnumerable<Point> AccessibleNeighbours(Point place)
        {
            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (this[place].CanGoThere[dir])
                {
                    yield return place.Add(dir);
                }
            }
        }

        private string GetRoomDescription()
        {
            if( streetNames.Count == 0)
            {
                return "A busy street in London.";
            }
            var result = Chance.RandomElement(streetNames);
            streetNames.Remove(result);
            return result.Substring(0, result.Length-1);
        }

        /// <summary>
        /// Create rooms next to the current one as long as there are still null neighbours
        /// </summary>
        /// <param name="fromPoint"></param>
        private void CreateNeighbour(Point fromPoint)
        {
            while (true)
            {
                List<Point> options = ListEmptyNeighbours(fromPoint);
                if (options.Count == 0)
                {
                    return;
                }
                Point destPoint = Chance.RandomElement(options);
                Rooms[destPoint.X, destPoint.Y] = new Room(GetRoomDescription(), destPoint);
                Connect(fromPoint, destPoint);
                CreateNeighbour(destPoint); //recursive step
            }
        }

        public IEnumerator<Room> GetEnumerator()
        {
            return new MazeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }
}
