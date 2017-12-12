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
        
        protected int MaxHealth = 100;

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
        public Character(string name, string description) : this (name, description, 100, 100)
        {

        }
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
