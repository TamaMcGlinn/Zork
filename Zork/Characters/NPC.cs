using Zork.Objects;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zork.Texts;
using System.Drawing;

namespace Zork.Characters
{
    public class NPC : Character
    {
        public int LetsPlayerFleePerXRounds { get; set; }
        public const int MinTurnsBetweenMoves = 2;
        public const int MaxTurnsBetweenMoves = 5;

        private int _turnsUntilNextMove;

        private TextTree _text;

        public TextTree Text
        {
            get { return _text; }
            protected set { _text = value; }
        }
        public NPC(string name, string description, int strength, int startHealth, int letsPlayerFleePerXRounds, Weapon weapon = null) : this(name, description, strength, startHealth, startHealth, letsPlayerFleePerXRounds, weapon)
        {

        }
        public Maze maze { get; set; }

        public NPC(string name, string description, int strength, int startHealth, int maxHealth,  int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, weapon)
        {
            this.Text = new TextTree(Name + ".txt");
            PickNextTimeToMove();
            LetsPlayerFleePerXRounds = letsPlayerFleePerXRounds;
        }

        private void PickNextTimeToMove()
        {
            var rng = new Random();
            _turnsUntilNextMove = rng.Next(MinTurnsBetweenMoves, MaxTurnsBetweenMoves + 1);
        }

        /// <summary>
        /// Output text and accept player choices until the tree reaches a leaf node
        /// </summary>
        public virtual void Talk(Player player)
        {
            Node currentNode = Node.FirstAvailable(Text.RootNodes, player);
            while (currentNode != null)
            {
                foreach(string s in currentNode.UnlockedClues)
                {
                    player.Clues.Add(s);
                }
                currentNode = ProcessNode(currentNode, player);
            }
        }

        private Node ProcessNode(Node currentNode, Player player)
        {
            Console.WriteLine(currentNode.Text);
            List<Node> options = currentNode.AvailableChildren(player);
            if (options.Count == 0)
            {
                return null;
            }
            Node playerResponse = GetPlayerResponse(currentNode, options);
            Console.WriteLine("> " + playerResponse.Text);
            return playerResponse.FirstAvailableChild(player);
        }

        public virtual void OnPlayerMoved(Game game)
        {
            _turnsUntilNextMove--;
            if( _turnsUntilNextMove == 0)
            {
                Move();
                PickNextTimeToMove();
            }
        }

        private void Move()
        {
            var rng = new Random();
            var currentLocation = CurrentRoom.LocationOfRoom;
            var options = maze.AccessibleNeighbours(currentLocation).ToList();
            if (options.Count > 0)
            {
                var newRoom = options[rng.Next(0, options.Count)];
                maze[currentLocation].NPCsInRoom.Remove(this);
                maze[newRoom].NPCsInRoom.Add(this);
                CurrentRoom = maze[newRoom];
            }
            PossiblyPickSomethingUp();
        }

        private void PossiblyPickSomethingUp()
        {
            Random rng = new Random();
            if (CurrentRoom.ObjectsInRoom.Count > 0)
            {
                int pickupIndex = rng.Next(0, CurrentRoom.ObjectsInRoom.Count);
                PickUpObject(CurrentRoom.ObjectsInRoom[pickupIndex]);
            }
            if( Inventory.Count >= 3)
            {
                int dropIndex = rng.Next(0, Inventory.Count);
                CurrentRoom.ObjectsInRoom.Add(Inventory[dropIndex]);
                Inventory.RemoveAt(dropIndex);
            }
        }

        private static Node GetPlayerResponse(Node currentNode, List<Node> options)
        {
            int responseNumber = 1;
            foreach (Node child in options)
            {
                Console.WriteLine(responseNumber + "> " + child.Text);
                ++responseNumber;
            }
            Console.Write("> ");
            int chosenResponse = -1;
            while (Int32.TryParse(Console.ReadLine(), out chosenResponse) == false || chosenResponse <= 0 || chosenResponse > currentNode.Children.Count)
            {
                Console.WriteLine("Write a number for one of the responses");
                Console.Write("> ");
            }
            Node playerResponse = options[chosenResponse - 1];
            return playerResponse;
        }

        public void KillThisNPC(Game game)
        {
            CurrentRoom.NPCsInRoom.Remove(this);
            DropAllItems();
            DropWeapon();
            CurrentRoom.ObjectsInRoom.Add(new CorpseNPCObject(this.Name));
            CurrentRoom = null;
            game.NPCS.Remove(this);
        }
    }
}
