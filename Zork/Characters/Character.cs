using System;
using System.Collections.Generic;
using Zork.Characters;
using Zork.Objects;
using Zork.UIContext;

namespace Zork
{
    public abstract class Character
    {
        #region properties
        
        public int MaxHealth = 100;

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
            set { _health = value; }
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
            Console.WriteLine();
        }

        protected void CheckWhoWon(NPC enemy, Game game)
        {
            if (Health < 0)
            {
                using (new ColorContext(ColorContext.BattleLose))
                {
                    Console.WriteLine("You died!");
                }
                Environment.Exit(0);
            }
            else
            {
                enemy.Inventory.AddRange(enemy.Inventory);
                enemy.KillThisNPC(game);
                bool gameWon = enemy is MurdererNPC;
                if (gameWon)
                {
                    using (new ColorContext(ColorContext.BattleWin))
                    {
                        Console.WriteLine($"You win! {enemy.Name} was served justice by death!");
                    }
                    Environment.Exit(0);
                }
                else
                {
                    using (new ColorContext(ColorContext.FailureColor))
                    {
                        Console.WriteLine($"Oh my god, you killed poor innocent {enemy.Name}! You've picked up all {enemy.Name}'s items, check your inventory!");
                    }
                }
            }
        }

        protected virtual void FightOneRound(NPC enemy)
        {
            int enemyDamage = enemy.GenerateDamage();
            int myDamage = GenerateDamage();
            enemy.TakeDamage(myDamage);
            TakeDamage(enemyDamage);
        }
        
        public int GenerateDamage()
        {
            const int maxBonusDamage = 10;
            int bonusDamage = Chance.Between(0, maxBonusDamage);

            int damage = Strength + bonusDamage;
            if (EquippedWeapon != null)
            {
                damage += EquippedWeapon.Strength;
            }
            return damage;
        }

        public void DropWeapon()
        {
            if (EquippedWeapon != null)
            {
                CurrentRoom.ObjectsInRoom.Add(EquippedWeapon);
                EquippedWeapon = null;
            }
        }

        public void DropAllItems()
        {
            CurrentRoom.ObjectsInRoom.AddRange(Inventory);
            Inventory.Clear();
        }

        protected void PickUpObject(BaseObject obj)
        {
            CurrentRoom.ObjectsInRoom.Remove(obj);
            obj.PickupObject(this);
        }
    }
}
