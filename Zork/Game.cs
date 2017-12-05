using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Drawing;
using System.Linq;

namespace Zork
{
    /// <summary>
    /// A Zork Game, which initialises the rooms, characters and objects, and allows you to play.
    /// </summary>
    public class Game
    {
        Room[,] allRooms;
        Point currentRoom;
        List<Character> allCharacters;
        const int Width = 20;
        const int Height = 20;
        const int StartX = 1;
        const int StartY = 1;
        Random rng = new Random();

        public Game()
        {
            createMaze();
            allCharacters = new List<Character>();
            var barney = new Character("sherrif_barney", 3, 100, null, "A fat man in a aprim black sherrif's uniform. He has a mustache and short brown hair.");
            allCharacters.Add(barney);
            allRooms[StartX, StartY].CharactersInRoom.Add(barney);
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
                    allRooms[a.X,a.Y].CanGoThere[Direction.South] = true;
                    allRooms[b.X,b.Y].CanGoThere[Direction.North] = true;
                } else
                {
                    Debug.Assert(a.Y - 1 == b.Y);
                    allRooms[a.X, a.Y].CanGoThere[Direction.North] = true;
                    allRooms[b.X, b.Y].CanGoThere[Direction.South] = true;
                }
            } else
            {
                if (a.X + 1 == b.X)
                {
                    allRooms[a.X, a.Y].CanGoThere[Direction.East] = true;
                    allRooms[b.X, b.Y].CanGoThere[Direction.West] = true;
                }
                else
                {
                    Debug.Assert(a.X - 1 == b.X);
                    allRooms[a.X, a.Y].CanGoThere[Direction.West] = true;
                    allRooms[b.X, b.Y].CanGoThere[Direction.East] = true;
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
                    if (allRooms[xi, yi].CanGoThere[Direction.East])
                    {
                        Debug.Assert(xi == Width-1 || allRooms[xi+1, yi].CanGoThere[Direction.West]);
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
                        if (allRooms[xi, yi].CanGoThere[Direction.South])
                        {
                            Debug.Assert(yi == Height - 1 || allRooms[xi, yi+1].CanGoThere[Direction.North]);
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
            } else if( allRooms[from.X,from.Y].CanGoThere[direction])
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
            printInstructions();
            while (true) {
                allRooms[currentRoom.X,currentRoom.Y].print();
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
                    case 'L':
                    case 'l':
                        Console.Write(allRooms[currentRoom.X, currentRoom.Y].LookAround());
                        break;
                    case 't':
                    case 'T':
                        var talkCommand = userInput.Split(' ');
                        if (talkCommand.Length >= 3 && talkCommand[1] == "to")
                        {
                            string charactername = string.Join("_", talkCommand.Skip(2)).ToLower();
                            Character talkTarget = allRooms[currentRoom.X, currentRoom.Y].CharactersInRoom.Find((Character c) => { return c.Name == charactername; });
                            if(talkTarget == null)
                            {
                                Console.WriteLine("There is nobody called " + charactername + " here.");
                            }
                            talkTarget.Talk();
                        } else
                        {
                            Console.WriteLine("Did you mean; \"Talk to [character name]\"?");
                        }
                        break;
                    default:
                        printInstructions();
                        break;
                }
            }
        }

        private void printInstructions()
        {
            Console.WriteLine("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around, [L] to look around");
        }
    }
}
