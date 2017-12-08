using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using Zork.Objects;

namespace Zork
{
    public class Maze
    {
        Room[,] rooms;
        Random rng;
        private int Width;
        private int Height;

        public Maze(int xSize, int ySize, int StartX, int StartY)
        {
            Width = xSize;
            Height = ySize;
            rng = new Random();
            rooms = new Room[xSize, ySize];
            rooms[StartX, StartY] = new Room("Your house");
            CreateNeighbour(new Point(StartX, StartY));
            AddExtraConnections(xSize * ySize);
        }

        public Room this[int x, int y]
        {
            get { return rooms[x, y]; }
        }

        public Room this[Point p]
        {
            get { return this[p.X, p.Y]; }
        }

        public void AddItemToRandomRoom(BaseObject obj)
        {
            GetRandomRoom().ObjectsInRoom.Add(obj);
        }

        public Room GetRandomRoom()
        {
            return this[rng.Next(0, Width), rng.Next(0, Height)];
        }

        public void Print()
        {
            for (int yi = 0; yi < Height; ++yi)
            {
                for (int xi = 0; xi < Width; ++xi)
                {
                    Console.Write("0");
                    if (rooms[xi, yi].CanGoThere[Direction.East])
                    {
                        Debug.Assert(xi == Width - 1 || rooms[xi + 1, yi].CanGoThere[Direction.West]);
                        Console.Write("-");
                    }
                    else if (xi < Width - 1)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
                if (yi < Height - 1)
                {
                    for (int xi = 0; xi < Width; ++xi)
                    {
                        if (rooms[xi, yi].CanGoThere[Direction.South])
                        {
                            Debug.Assert(yi == Height - 1 || rooms[xi, yi + 1].CanGoThere[Direction.North]);
                            Console.Write("|");
                        }
                        else if (xi < Width - 1)
                        {
                            Console.Write(" ");
                        }
                        Console.Write(" ");
                    }
                    Console.Write("\n");
                }
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
                int neighbourToConnect = rng.Next(0, 4);
                var neighbourPoint = new Point(roomToConnect.X + (neighbourToConnect % 2) * ((neighbourToConnect / 2) * 2 - 1), roomToConnect.Y + (1 - (neighbourToConnect % 2)) * ((neighbourToConnect / 2) * 2 - 1));
                Connect(roomToConnect, neighbourPoint);
            }
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
                if (a.Y + 1 == b.Y)
                {
                    rooms[a.X, a.Y].CanGoThere[Direction.South] = true;
                    rooms[b.X, b.Y].CanGoThere[Direction.North] = true;
                }
                else
                {
                    Debug.Assert(a.Y - 1 == b.Y);
                    rooms[a.X, a.Y].CanGoThere[Direction.North] = true;
                    rooms[b.X, b.Y].CanGoThere[Direction.South] = true;
                }
            }
            else
            {
                if (a.X + 1 == b.X)
                {
                    rooms[a.X, a.Y].CanGoThere[Direction.East] = true;
                    rooms[b.X, b.Y].CanGoThere[Direction.West] = true;
                }
                else
                {
                    Debug.Assert(a.X - 1 == b.X);
                    rooms[a.X, a.Y].CanGoThere[Direction.West] = true;
                    rooms[b.X, b.Y].CanGoThere[Direction.East] = true;
                }
            }
        }

        /// <summary>
        /// Return the neighbouring points for which the room is null.
        /// </summary>
        /// <param name="place">The target location to examine</param>
        /// <returns></returns>
        private List<Point> ListEmptyNeighbours(Point place)
        {
            List<Point> result = new List<Point>();
            if (place.X > 0 && rooms[place.X - 1, place.Y] == null)
            {
                result.Add(new Point(place.X - 1, place.Y));
            }
            if (place.Y > 0 && rooms[place.X, place.Y - 1] == null)
            {
                result.Add(new Point(place.X, place.Y - 1));
            }
            if (place.X < Width - 1 && rooms[place.X + 1, place.Y] == null)
            {
                result.Add(new Point(place.X + 1, place.Y));
            }
            if (place.Y < Height - 1 && rooms[place.X, place.Y + 1] == null)
            {
                result.Add(new Point(place.X, place.Y + 1));
            }
            return result;
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
                if (options.Count > 0)
                {
                    Point destPoint = options[rng.Next(0, options.Count)];
                    rooms[destPoint.X, destPoint.Y] = new Room("A busy street in London.");
                    Connect(fromPoint, destPoint);
                    CreateNeighbour(destPoint); //recursive step
                }
                else
                {
                    break;
                }
            }
        }

    }
}
