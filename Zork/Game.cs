﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    class Game
    {
        Room[,] allRooms;
        Coordinates currentRoom;
        const int Width = 20;
        const int Height = 20;

        public Game()
        {
            allRooms = new Room[Width,Height];
            allRooms[0, 0] = new Room("You are in a busy street in London.");
            currentRoom = new Coordinates( 0, 0 );
        }

        public void Run()
        {
            while (true) {
                allRooms[currentRoom.x,currentRoom.y].Print();
                string userInput = Console.ReadLine();
                switch (userInput[0])
                {
                    case 'n':
                    case 'N':
                        if (currentRoom.y == 0)
                        {
                            Console.WriteLine("You attempt to go north but face the end of the world.");
                        }
                        else if (allRooms[currentRoom.x, currentRoom.y - 1] == null)
                        {
                            Console.WriteLine("There is nothing to the north.");
                        }
                        else
                        {
                            currentRoom.y--;
                        }
                        break;
                    case 's':
                    case 'S':
                        if (currentRoom.y == Height - 1)
                        {
                            Console.WriteLine("You attempt to go south but face the end of the world.");
                        }
                        else if (allRooms[currentRoom.x, currentRoom.y + 1] == null)
                        {
                            Console.WriteLine("There is nothing to the south.");
                        }
                        else
                        {
                            currentRoom.y++;
                        }
                        break;
                    case 'e':
                    case 'E':
                        if (currentRoom.x == Width - 1)
                        {
                            Console.WriteLine("You attempt to go east but face the end of the world.");
                        }
                        else if (allRooms[currentRoom.x + 1, currentRoom.y] == null)
                        {
                            Console.WriteLine("There is nothing to the east.");
                        }
                        else
                        {
                            currentRoom.x++;
                        }
                        break;
                    case 'w':
                    case 'W':
                        if (currentRoom.x == 0)
                        {
                            Console.WriteLine("You attempt to go west but face the end of the world.");
                        }
                        else if (allRooms[currentRoom.x - 1, currentRoom.y] == null)
                        {
                            Console.WriteLine("There is nothing to the west.");
                        }
                        else
                        {
                            currentRoom.x++;
                        }
                        break;
                    default:
                        Console.Write("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around");
                        break;
                }
            }
        }
    }
}
