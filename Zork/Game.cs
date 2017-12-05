using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Zork
{
    /// <summary>
    /// A Zork Game, which initialises the rooms, characters and objects, and allows you to play.
    /// </summary>
    public class Game
    {
        Room[,] allRooms;
        Point currentRoom;
        const int Width = 20;
        const int Height = 20;
        const int StartX = 1;
        const int StartY = 1;
        Random rng = new Random();

        public Game()
        {
            createMaze();
        }

        /// <summary>
        /// Create a maze with at least one path between every two points.
        /// </summary>
        private void createMaze()
        {
            allRooms = new Room[Width, Height];
            currentRoom = new Point(StartX, StartY);
            allRooms[StartX, StartY] = new Room("Your house");
            createNeighbour(currentRoom);
            addExtraConnections(Width*Height);
            printWholeMap();
        }

        private void addExtraConnections(int extras)
        {
            for(int i = 0; i < extras; ++i)
            {
                var roomToConnect = new Point(rng.Next(1, Width - 1), rng.Next(1, Height - 1));
                int neighbourToConnect = rng.Next(0, 4);
                var neighbourPoint = new Point(roomToConnect.X + (neighbourToConnect % 2)*((neighbourToConnect / 2) * 2 - 1), roomToConnect.Y + (1 - (neighbourToConnect % 2)) * ((neighbourToConnect / 2) * 2 - 1));
                connect(roomToConnect, neighbourPoint);
            }
        }

        /// <summary>
        /// Set the canGoThere variable to true between the two locations.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void connect(Point a, Point b)
        {
            if(a.X == b.X)
            {
                if(a.Y+1 == b.Y)
                {
                    allRooms[a.X,a.Y].canGoThere[Direction.South] = true;
                    allRooms[b.X,b.Y].canGoThere[Direction.North] = true;
                } else
                {
                    Debug.Assert(a.Y - 1 == b.Y);
                    allRooms[a.X, a.Y].canGoThere[Direction.North] = true;
                    allRooms[b.X, b.Y].canGoThere[Direction.South] = true;
                }
            } else
            {
                if (a.X + 1 == b.X)
                {
                    allRooms[a.X, a.Y].canGoThere[Direction.East] = true;
                    allRooms[b.X, b.Y].canGoThere[Direction.West] = true;
                }
                else
                {
                    Debug.Assert(a.X - 1 == b.X);
                    allRooms[a.X, a.Y].canGoThere[Direction.West] = true;
                    allRooms[b.X, b.Y].canGoThere[Direction.East] = true;
                }
            }
        }

        private void printWholeMap()
        {
            for (int yi = 0; yi < Height; ++yi)
            {
                for (int xi = 0; xi < Width; ++xi)
                {
                    Console.Write("0");
                    if (allRooms[xi, yi].canGoThere[Direction.East])
                    {
                        Debug.Assert(xi == Width-1 || allRooms[xi+1, yi].canGoThere[Direction.West]);
                        Console.Write("-");
                    } else if(xi < Width - 1)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
                if( yi < Height-1)
                {
                    for (int xi = 0; xi < Width; ++xi)
                    {
                        if (allRooms[xi, yi].canGoThere[Direction.South])
                        {
                            Debug.Assert(yi == Height - 1 || allRooms[xi, yi+1].canGoThere[Direction.North]);
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

        /// <summary>
        /// Return the neighbouring points for which the room is null.
        /// </summary>
        /// <param name="place">The target location to examine</param>
        /// <returns></returns>
        private List<Point> listEmptyNeighbours(Point place)
        {
            List<Point> result = new List<Point>();
            if(place.X > 0 && allRooms[place.X-1,place.Y] == null)
            {
                result.Add(new Point(place.X - 1, place.Y));
            }
            if (place.Y > 0 && allRooms[place.X, place.Y - 1] == null)
            {
                result.Add(new Point(place.X, place.Y - 1));
            }
            if(place.X < Width-1 && allRooms[place.X+1,place.Y] == null)
            {
                result.Add(new Point(place.X + 1, place.Y));
            }
            if (place.Y < Height-1 && allRooms[place.X, place.Y + 1] == null)
            {
                result.Add(new Point(place.X, place.Y + 1));
            }
            return result;
        }

        /// <summary>
        /// Create rooms next to the current one as long as there are still null neighbours
        /// </summary>
        /// <param name="fromPoint"></param>
        private void createNeighbour(Point fromPoint)
        {
            List<Point> options;
            do
            {
                options = listEmptyNeighbours(fromPoint);
                if (options.Count > 0)
                {
                    Point destPoint = options[rng.Next(0, options.Count)];
                    allRooms[destPoint.X, destPoint.Y] = new Room("A busy street in London.");
                    connect(fromPoint, destPoint);
                    createNeighbour(destPoint); //recursive step
                }
            } while (options.Count > 0);
        }

        /// <summary>
        /// Attempt to go from the from point to the towards point.
        /// </summary>
        /// <param name="from">From point</param>
        /// <param name="towards">Destination point</param>
        /// <param name="direction">Direction from from to towards</param>
        private void tryGo(Point from, Point towards, Direction direction)
        {
            if( towards.X < 0 || towards.X == Width || towards.Y < 0 || towards.Y == Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            } else if( allRooms[from.X,from.Y].canGoThere[direction])
            {
                currentRoom = towards;
            } else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }

        /// <summary>
        /// Print the room, get user input to accept commands
        /// </summary>
        public void run()
        {
            while (true) {
                allRooms[currentRoom.X,currentRoom.Y].Print();
                string userInput = Console.ReadLine();
                switch (userInput[0])
                {
                    case 'n':
                    case 'N':
                        tryGo(currentRoom, new Point(currentRoom.X, currentRoom.Y-1), Direction.North);
                        break;
                    case 's':
                    case 'S':
                        tryGo(currentRoom, new Point(currentRoom.X, currentRoom.Y + 1), Direction.South);
                        break;
                    case 'e':
                    case 'E':
                        tryGo(currentRoom, new Point(currentRoom.X + 1, currentRoom.Y), Direction.East);
                        break;
                    case 'w':
                    case 'W':
                        tryGo(currentRoom, new Point(currentRoom.X - 1, currentRoom.Y), Direction.West);
                        break;
                    default:
                        Console.Write("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around");
                        break;
                }
            }
        }
    }
}
