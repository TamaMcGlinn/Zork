using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;
using Zork.Texts;

namespace Zork.Characters
{
    public class NPC : Character
    {
        public const int MinTurnsBetweenMoves = 2;
        public const int MaxTurnsBetweenMoves = 5;

        private int _turnsUntilNextMove;

        private TextTree _text;

        public TextTree Text
        {
            get { return _text; }
            protected set { _text = value; }
        }

        public NPC(string name, string description, int strength, int startHealth, Weapon weapon = null) : this(name, description, strength, startHealth, startHealth, weapon)
        {
        }

        public NPC(string name, string description, int strength, int startHealth, int maxHealth, Weapon weapon = null) : base(name, description, strength, startHealth, maxHealth, weapon)
        {
            this.Text = new TextTree(Name + ".txt");
            PickNextTimeToMove();
        }

        public void PickNextTimeToMove()
        {
            var rng = new Random();
            _turnsUntilNextMove = rng.Next(MinTurnsBetweenMoves, MaxTurnsBetweenMoves + 1);
        }

        /// <summary>
        /// Output text and accept player choices until the tree reaches a leaf node
        /// </summary>
        public void Talk()
        {
            Node currentNode = Text.RootNode;
            while (currentNode != null)
            {
                currentNode = ProcessNode(currentNode);
            }
        }

        public bool IsTimeToMove()
        {
            return _turnsUntilNextMove == 0;
        }

        public void PlayerMoved()
        {
            _turnsUntilNextMove--;
        }

        private Node ProcessNode(Node currentNode)
        {
            Console.WriteLine(currentNode.Text);
            List<Node> options = currentNode.AvailableChildren();
            if (options.Count == 0)
            {
                return null;
            }
            Node playerResponse = GetPlayerResponse(currentNode, options);
            Console.WriteLine("> " + playerResponse.Text);
            List<Node> npcResponses = playerResponse.AvailableChildren();
            if (npcResponses.Count == 0)
            {
                return null;
            }
            return npcResponses.First();
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
            while (Int32.TryParse(Console.ReadLine(), out chosenResponse) == false || chosenResponse < 0 || chosenResponse > currentNode.Children.Count)
            {
                Console.WriteLine("Write a number for one of the responses");
                Console.Write("> ");
            }
            Node playerResponse = options[chosenResponse - 1];
            return playerResponse;
        }
    }
}
