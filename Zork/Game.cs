﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Zork.Characters;
using Zork.Extensions;
using Zork.Objects;

namespace Zork
{
    /// <summary>
    /// A Zork Game, which initialises the rooms, characters and objects, and allows you to play.
    /// </summary>
    public class Game
    {
        Maze maze;
        Point currentRoom;
        const int Width = 20;
        const int Height = 20;
        const int StartX = 1;
        const int StartY = 1;

        Dictionary<char, Action<Game, string>> Commands = new Dictionary<char, Action<Game, string>>()
        {
            { 'n', (Game g, string s) => { g.TryGo(Direction.North); } },
            { 'e', (Game g, string s) => { g.TryGo(Direction.East); } },
            { 's', (Game g, string s) => { g.TryGo(Direction.South); } },
            { 'w', (Game g, string s) => { g.TryGo(Direction.West); } },
            { 'l', (Game g, string s) => { Console.Write(g.maze[g.currentRoom].LookAround()); } },
            { 't', (Game g, string s) => { g.tryTalk(s); } },
            { 'p', (Game g, string s) => { Interactions.PickupItem(g.maze, g.currentRoom); } },
            { 'i', (Game g, string s) => { CharacterDefinitions.PlayerCharacter.PrintInventory(); } },
            { 'c', (Game g, string s) => { CharacterDefinitions.PlayerCharacter.PrintStats(); } },
            { 'b', (Game g, string s) => { g.Battle(); } }
        };

        public Game()
        {
            currentRoom = new Point(StartX, StartY);
            maze = new Maze(Width, Height, StartX, StartY);
            maze.Print();
            CharacterDefinitions.AddCharacters(maze);
            ObjectDefinitions.AddItems(maze);
        }

        /// <summary>
        /// Print the room, get user input to accept commands
        /// </summary>
        public void Run()
        {
            PrintInstructions();
            while (true)
            {
                maze[currentRoom].Print();
                ProcessInput(Console.ReadLine());
            }
        }

        private void tryTalk(string userInput)
        {
            var talkCommand = userInput.Split(' ');
            if (talkCommand.Length >= 3 && talkCommand[1] == "to")
            {
                string charactername = string.Join("_", talkCommand.Skip(2)).ToLower();
                Character talkTarget = maze[currentRoom].CharactersInRoom.Find((Character c) => { return c.Name == charactername; });
                if (talkTarget == null)
                {
                    Console.WriteLine("There is nobody called " + charactername + " here.");
                }
                talkTarget.Talk();
            }
            else
            {
                Console.WriteLine("Did you mean; \"Talk to [character name]\"?");
            }
        }

        /// <summary>
        /// <summary>me="direction">Direction to go in</param>
        /// </summary>
        /// <param name="currentRoom">The room you're in</param>
        /// <param name="direction">The direction to go in</param>
        private void TryGo(Direction direction)
        {
            Point towards = currentRoom.Add(direction);
            if (towards.X < 0 || towards.X == Width || towards.Y < 0 || towards.Y == Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            }
            else if (maze[currentRoom].CanGoThere[direction])
            {
                currentRoom = towards;
            }
            else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }

        private void ProcessInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput.Length > 0)
            {
                Action<Game,string> action = Commands[userInput[0]];
                if (action != null)
                {
                    action(this, userInput);
                    return;
                }
            }
            PrintInstructions();
        }

        private void Battle()
        {
            Character enemy = Interactions.ChooseEnemy(maze, currentRoom);
            if (enemy != null)
            {
                Interactions.Fight(enemy, CharacterDefinitions.PlayerCharacter);
            }
        }

        /// <summary>
        /// Lists all items in the room and gives options for the player to pick them up. 
        /// If he chooses a valid item it gets added to the inventory.
        /// </summary>
        private void PickupItem()
        {
            if (maze[currentRoom].ObjectsInRoom.Count <= 0)
            {
                Console.WriteLine("There are no items to pickup in this room.");
                return;
            }
            for (int i = 0; i < maze[currentRoom].ObjectsInRoom.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] to pickup:" + maze[currentRoom].ObjectsInRoom[i].Name);
            }
            string input = Console.ReadLine();
            int inputInteger;
            int.TryParse(input, out inputInteger);
            TryPickUp(inputInteger - 1);
        }

        private void TryPickUp(int choiceIndex)
        {
            if (choiceIndex >= 0 && choiceIndex < maze[currentRoom].ObjectsInRoom.Count)
            {
                var obj = maze[currentRoom].ObjectsInRoom[choiceIndex];
                maze[currentRoom].ObjectsInRoom.Remove(obj);
                obj.PickupObject(CharacterDefinitions.PlayerCharacter);
            }
            else
            {
                Console.WriteLine("Cannot pick that item up.");
            }
        }

        private void PrintInstructions()
        {
            Console.WriteLine("Please enter [N]orth, [S]outh, [E]ast or [W]est to move around.");
            Console.WriteLine("[L] to look around, [P] to pick up an item, [I] Inventory, [B] Battle");
            Console.WriteLine("[C] to view stats");
        }
    }
}
