using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    class Game
    {
        Room currentRoom;

        public Game()
        {
            currentRoom = new Room("You are in a busy street in London.");
        }

        public void Run()
        {
            currentRoom.Print();
            string userInput = Console.ReadLine();
            switch (userInput[0])
            {
                case 'N':
                    throw new Exception("You went North!");
            }
        }
    }
}
