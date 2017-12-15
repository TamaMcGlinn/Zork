using Zork.Objects;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zork.Texts;

namespace Zork.Characters
{
    public class NPC : Character
    {
        private bool _isHostile = false;

        public bool IsHostile
        {
            get { return _isHostile; }
            private set { _isHostile = value; }
        }

        private const int PickUpChance = 10;

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
        public Maze Maze { get; set; }

        public NPC(string name, string description, int strength, int startHealth, int maxHealth,  int letsPlayerFleePerXRounds, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, weapon)
        {
            this.Text = new TextTree(Name);
            PickNextTimeToMove();
            LetsPlayerFleePerXRounds = letsPlayerFleePerXRounds;
        }

        private void PickNextTimeToMove()
        {
            _turnsUntilNextMove = Chance.Between(MinTurnsBetweenMoves, MaxTurnsBetweenMoves + 1);
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

        public void OnPlayerMoved(Game game)
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
            var currentLocation = CurrentRoom.LocationOfRoom;
            var options = Maze.AccessibleNeighbours(currentLocation).ToList();
            if (options.Count > 0)
            {
                var newRoom = Chance.RandomElement(options);
                Maze[currentLocation].NPCsInRoom.Remove(this);
                Maze[newRoom].NPCsInRoom.Add(this);
                CurrentRoom = Maze[newRoom];
            }
            PossiblyPickSomethingUp();
        }

        private void PossiblyPickSomethingUp()
        {
            List<BaseObject> NonClueItems = CurrentRoom.GetNonClueItems();
            if (NonClueItems.Count > 0)
            {
                if (Chance.Percentage(PickUpChance))
                {
                    PickUpObject(Chance.RandomElement(NonClueItems));
                }
            }
            if( Inventory.Count >= 3)
            {
                var item = Chance.RandomElement(Inventory);
                CurrentRoom.ObjectsInRoom.Add(item);
                Inventory.Remove(item);
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

        public void MarkHostile()
        {
            IsHostile = true;
        }
    }
}
