using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;
using Zork.Texts;
using System.Drawing;
using System.Diagnostics;

namespace Zork
{
    public abstract class Character
    {
        #region properties

        protected int MaxHealth = 100;

        private string _name;

        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        protected Point _location;

        public Point Location
        {
            get { return _location; }
        }


        private int _strength;

        public int Strength
        {
            get { return _strength; }
            protected set { _strength = value; }
        }

        private int _health;

        public int Health
        {
            get { return _health; }
            protected set { _health = value; }
        }


        private Weapon _weapon;

        public Weapon EquippedWeapon
        {
            get { return _weapon; }
            set { _weapon = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }
        
        private TextTree _text;

        public TextTree Text
        {
            get { return _text; }
            protected set { _text = value; }
        }
        
        private List<Objects.BaseObject> _inventory = new List<BaseObject>();


        public List<BaseObject> Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        #endregion properties

        public Character(string name, string description, int strength, int startHealth, Weapon weapon = null) : this(name, description, strength, startHealth, startHealth, weapon)
        {
        }

        /// <summary>
        /// Character constructor with full options for parameters
        /// </summary>
        /// <param name="name">The name of the character</param>
        /// <param name="strength">The characters strength</param>
        /// <param name="health">The characters health</param>
        /// <param name="weapon">The weapon the character has equipped</param>
        /// <param name="location">Current location of the character</param>
        /// <param name="description">a description of what the character looks like</param>
        public Character(string name, string description, int strength, int startHealth, int maxHealth, Weapon weapon = null)
        {
            this.Name = name;
            this.Description = description;
            this.Strength = strength;
            this.MaxHealth = maxHealth;
            this.Health = Math.Min(maxHealth, startHealth);
            this.EquippedWeapon = weapon;
            SetTextTree();
        }

        /// <summary>
        /// Lists all items in the character's inventory
        /// </summary>
        public void PrintInventory()
        {
            if (Inventory.Count == 0)
            {
                Console.WriteLine("\nYou have no items in your inventory.\n");
                return;
            }

            Console.WriteLine("You currently have the following items:");
            for (int i = 0; i < Inventory.Count; i++)
            {
                Console.WriteLine($"{Inventory[i].Name} : {Inventory[i].Description}");
            };
        }
        
        /// Take the specified damage, return whether we are still alive
        /// </summary>
        /// <param name="damage">hitpoints to remove</param>
        /// <returns>whether we are still alive</returns>
        public bool takeDamage(int damage)
        {
            Health -= damage;
            return Health > 0;
        }

        private void SetTextTree()
        {

            this.Text = new TextTree(Name + ".txt");
        }

        /// <summary>
        /// Output text and accept player choices until the tree reaches a leaf node
        /// </summary>
        public void Talk()
        {
            Node currentNode = Text.RootNode;
            while (true)
            {
                Console.WriteLine(currentNode.Text);
                List<Node> options = currentNode.AvailableChildren();
                if (options.Count == 0)
                {
                    return;
                }

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
                Console.WriteLine("> " + playerResponse.Text);
                List<Node> npcResponses = playerResponse.AvailableChildren();
                if (npcResponses.Count > 0)
                {
                    currentNode = npcResponses.First();
                } else
                {
                    return;
                }
            }
        }

        public void PrintStats()
        {
            Console.WriteLine(Name + ": " + Description);
            Console.WriteLine("Health: " + Health);
            Console.WriteLine("Strength: " + Strength);
            if (EquippedWeapon == null)
            {
                Console.WriteLine("Unarmed.");
            }
            else
            {
                Console.WriteLine("Current weapon:");
                EquippedWeapon.PrintStats();
            }
        }

        /// <summary>
        /// Attempt to go from the from point to the towards point.
        /// </summary>
        /// <param name="from">From point</param>
        /// <param name="towards">Destination point</param>
        /// <param name="direction">Direction from from to towards</param>
        public void TryGo(Maze maze, Point towards, Direction direction)
        {
            if (towards.X < 0 || towards.X == maze.Width || towards.Y < 0 || towards.Y == maze.Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            }
            else if (maze[_location].CanGoThere[direction])
            {
                _location = towards;
            }
            else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }
    }
}
