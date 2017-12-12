using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Zork.Behaviour;
using Zork.Objects;

namespace Zork.Characters
{
    public class Player : Character
    {
        #region properties

        private List<string> _cluesFound = new List<string>();

        public List<string> Clues
        {
            get { return _cluesFound; }
        }
        #endregion

        public Player() : base()
        {
            Name = "Sherlock Holmes";
            Description = "A very good investigator.";
            EquippedWeapon = null;
            Strength = 10;
            Health = MaxHealth;
        }

        public void UseHealthPickup(HealthPickup h)
        {
            Health = Math.Min(MaxHealth, Health + h.Potency);
        }

        private string PrintHittingWithWeapon()
        {
            if (EquippedWeapon != null)
            {
                Console.Write($" with your mighty {EquippedWeapon.Name} ");
            }
            return "";
        }

        private string PrintGetHittedWithWeapon()
        {
            if (EquippedWeapon != null)
            {
                Console.Write($" with his stupid {Enemy.EquippedWeapon.Name} ");
            }
            return "";
        }

        protected override void FightOneRound()
        {
            int enemyDamage = Enemy.GenerateDamage();
            int myDamage = GenerateDamage();

            Console.WriteLine("You hit eachother...");
            Enemy.TakeDamage(myDamage);
            TakeDamage(enemyDamage);

            Console.Write("You hit for: " + myDamage + PrintHittingWithWeapon());
            Console.Write($"\n{Enemy.Name} hits you for:" + enemyDamage + PrintGetHittedWithWeapon());
            Console.WriteLine($"\nYou have {Health} hp left, he has {Enemy.Health} hp left.");
        }

        public override BattleOutcomeEnum Fight(Character enemy, Room[,] allRooms)
        {
            Enemy = enemy;
            while (enemy.Health > 0 && Health > 0 && !Fled)
            {
                if (enemy is NPC) {
                    if (TurnsPassed % (enemy as NPC).LetsPlayerFleePerXRounds == 0)
                    {
                        AskFlee();
                    }
                 }
                FightOneRound();
                Turn();
            }
            if (Fled)
            {
                Flee(allRooms);
                return BattleOutcomeEnum.PlayerFled;
            }
            return base.CheckWhoWon();
        }


        public bool Fled { get; set; } = false;


        protected void AskFlee()
        {
            Console.WriteLine("Do you want to flee? Y/N");
            char userInputCharacter = (char)Console.Read();
            if (userInputCharacter == 'y' || userInputCharacter == 'Y')
            {
                Fled = true;
            }
            else
            {
                Fled = false;
            }
        }

        /// <summary>
        /// Lets your character run to a random room
        /// </summary>
        /// <param name="allRooms"></param>
        public void Flee(Room[,] allRooms)
        {
            Point newRoom;
            if (CurrentRoom != null)
            {
                newRoom =  CurrentRoom.LocationOfRoom; 
                while (newRoom.X == CurrentRoom.LocationOfRoom.X && newRoom.Y == CurrentRoom.LocationOfRoom.Y)
                {
                    newRoom = getRandomPointWithinGameBounds();
                }
            }
            else
            {
               newRoom = getRandomPointWithinGameBounds();
            }

            foreach(Room r in allRooms)
            {
                if(r.LocationOfRoom.X == newRoom.X && r.LocationOfRoom.Y == newRoom.Y)
                {
                    CurrentRoom = r;
                }
            }

            Console.WriteLine("...What ...Where am i?");
        }

        public Point getRandomPointWithinGameBounds()
        {
            Random r = new Random();
            int X, Y;
            X = r.Next(0, Game.Width);
            Y = r.Next(0, Game.Height);
            return new Point(X, Y);
        }
    }
}
