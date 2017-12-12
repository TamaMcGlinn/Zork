using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Zork.Behaviour;
using Zork.Objects;
using Zork.Texts;

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

        private Room _currentRoom;

        public Room CurrentRoom
        {
            get { return _currentRoom; }
            set { _currentRoom = value; }
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


        public void PrintEquippedWeapon()
        {
            if (EquippedWeapon != null)
            {
                Console.WriteLine($"Youre holding a {EquippedWeapon.Name} :  {EquippedWeapon.Description}");
            }
            else
            {
                Console.WriteLine("You're not holding a weapon");
            }
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

        /// <summary>
        /// Sets the texttree for this character so that the player can engage in a conversation with the character
        /// </summary>
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
            while (currentNode != null)
            {
                currentNode = ProcessNode(currentNode);
            }
        }

        private Node ProcessNode(Node currentNode)
        {
            Console.WriteLine(currentNode.Text);
            List<Node> options = currentNode.AvailableChildren(this);
            if (options.Count == 0)
            {
                return null;
            }
            Node playerResponse = GetPlayerResponse(currentNode, options);
            Console.WriteLine("> " + playerResponse.Text);
            List<Node> npcResponses = playerResponse.AvailableChildren(this);
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



        public Character Enemy { get; set; }

        internal int TurnsPassed { get; set; }

        /// <summary>
        /// Fights the chosen enemy untill someone dies, if player dies he loses all his items, 
        /// if enemy dies player picks up all his items.
        /// </summary>
        /// <returns>A boolean indicating wether the player won the fight</returns>
        public virtual BattleOutcomeEnum Fight(Character enemy, Room[,] AllRooms)
        {
            Enemy = enemy;
            while (enemy.Health > 0 && Health > 0)
            {
                FightOneRound();
                Turn();
            }
            return CheckWhoWon();
        }

        public void Turn()
        {
            TurnsPassed++;
        }

        protected BattleOutcomeEnum CheckWhoWon()
        {
            if (Enemy.Health < 0)
            {
                Enemy.Inventory.Clear();
                Enemy.ResetHealth();
                Console.WriteLine("You died! But luckily you've returned without items.");
                return BattleOutcomeEnum.EnemyWon;
            }
            else
            {
                Enemy.Inventory.AddRange(Enemy.Inventory);
                Console.WriteLine($"You've won! You've picked up all {Enemy.Name}'s items, check your inventory!");
                return BattleOutcomeEnum.PlayerWon;
            }
        }

        protected virtual void FightOneRound()
        {
            int enemyDamage = Enemy.GenerateDamage();
            int myDamage = GenerateDamage();
            Enemy.TakeDamage(myDamage);
            TakeDamage(enemyDamage);
        }
        
        public int GenerateDamage()
        {
            Random turnBonusDamageGenerator = new Random();
            const int maxBonusDamage = 10;
            int bonusDamage = turnBonusDamageGenerator.Next(0, maxBonusDamage);

            int damage = Strength + bonusDamage;
            if (EquippedWeapon != null)
            {
                damage += EquippedWeapon.Strength;
            }
            return damage;
        }

       
    }
}
