using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Zork.Objects;

namespace Zork
{
    public class Maze
    {
        public Room[,] Rooms { get; set; }
        Random rng;
        public readonly int Width;
        public readonly int Height;

        public Maze(int xSize, int ySize, int StartX, int StartY)
        {
            Width = xSize;
            Height = ySize;
            rng = new Random();
            Rooms = new Room[xSize, ySize];
            Rooms[StartX, StartY] = new Room("Your house", new Point(StartX, StartY));
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
            return new Point(rng.Next(0, Width), rng.Next(0, Height));
        }

        public void Print()
        {
            for (int yi = 0; yi < Height; ++yi)
            {
                for (int xi = 0; xi < Width; ++xi)
                {
                    PrintHorizontal(xi, yi);
                }
                PrintLowerHalf(yi);
            }
        }

        private void PrintLowerHalf(int yi)
        {
            Console.Write("\n");
            if (yi < Height - 1)
            {
                for (int xi = 0; xi < Width; ++xi)
                {
                    PrintVertical(xi, yi);
                }
                Console.Write("\n");
            }
        }

        private void PrintVertical(int xi, int yi)
        {
            if (Rooms[xi, yi].CanGoThere[Direction.South])
            {
                Debug.Assert(yi == Height - 1 || Rooms[xi, yi + 1].CanGoThere[Direction.North]);
                Console.Write("|");
            }
            else if (xi < Width - 1)
            {
                Console.Write(" ");
            }
            Console.Write(" ");
        }

        private void PrintHorizontal(int xi, int yi)
        {
            Console.Write("0");
            if (Rooms[xi, yi].CanGoThere[Direction.East])
            {
                Debug.Assert(xi == Width - 1 || Rooms[xi + 1, yi].CanGoThere[Direction.West]);
                Console.Write("-");
            }
            else if (xi < Width - 1)
            {
                Console.Write(" ");
            }
        }

        private void AddExtraConnections(int extras)
        {
            if(Width < 3 || Height < 3)
            {
                return;
            }
            for (int i = 0; i < extras; ++i)
            {
                var roomToConnect = new Point(rng.Next(1, Width - 1), rng.Next(1, Height - 1));
                var neighbourPoint = GetRandomNeighbour(roomToConnect);
                Connect(roomToConnect, neighbourPoint);
            }
        }

        public Point GetRandomNeighbour(Point roomToConnect)
        {
            Direction[] allDirections = (Direction[])Enum.GetValues(typeof(Direction));
            Direction direction = allDirections[rng.Next(0, allDirections.Length)];
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
            return otherRooms[rng.Next(0, otherRooms.Count)];
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
                Point destPoint = options[rng.Next(0, options.Count)];
                Rooms[destPoint.X, destPoint.Y] = new Room("A busy street in London.", new Point(destPoint.X, destPoint.Y));
                Connect(fromPoint, destPoint);
                CreateNeighbour(destPoint); //recursive step
            }
        }

    }
}
