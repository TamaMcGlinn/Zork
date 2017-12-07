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

        protected static readonly int MaxHealth = 100;

        private string _name;

        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
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



        public Character()
        {

        }

        /// <summary>
        /// Ctor with all default values except name and description
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Character(string name, string description) : this (name, 5, 100, null, description)
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
        public Character(string name, int strength, int health, Weapon weapon, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Strength = strength;
            this.Health = health;
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

        public void ResetHealth()
        {
            Health = MaxHealth;
        }

        /// Take the specified damage, return whether we are still alive
        /// </summary>
        /// <param name="damage">hitpoints to remove</param>
        /// <returns>whether we are still alive</returns>
        public bool TakeDamage(int damage)
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
    }
}
