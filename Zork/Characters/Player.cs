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
        
        public HashSet<string> Clues = new HashSet<string>();
        
        #endregion

        public Player(Room currentRoom) : base()
        {
            Name = "Sherlock Holmes";
            Description = "A very good investigator.";
            EquippedWeapon = null;
            Strength = 10;
            Health = MaxHealth;
            CurrentRoom = currentRoom;
        } 

        /// <summary>
        /// Prints what the player sees
        /// </summary>
        public void LookAround()
        {
            string lookAroundString = CurrentRoom.PrintLookAroundString();
            Console.Write(lookAroundString);
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

        public override BattleOutcomeEnum Fight(NPC enemy, Room[,] allRooms)
        {
            Enemy = enemy;
            while (enemy.Health > 0 && Health > 0 && !Fled)
            {                
                if (TurnsPassed % enemy.LetsPlayerFleePerXRounds == 0)
                {
                    AskFlee();
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

        #region talkMethods

        public void TryTalk()
        {
            Console.WriteLine("With who do you want to talk?");
            List<NPC> npcs =  CurrentRoom.NPCsInRoom;
            for (int i = 0; i < npcs.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {npcs[i].Name}");
            }
            int talkToNPCInt;
            int.TryParse(Console.ReadLine(), out talkToNPCInt);
            talkToNPCInt -= 1;
            if(talkToNPCInt >= 0 && talkToNPCInt <= npcs.Count)
            {
                npcs[talkToNPCInt].Talk(this);
            }
            else
            {
                Console.WriteLine("He's not here...");
                return;
            }
        }

        #endregion

        /// <summary>
        /// <summary>me="direction">Direction to go in</param>
        /// </summary>
        /// <param name="currentRoom">The room you're in</param>
        /// <param name="direction">The direction to go in</param>
        public void TryGo(Direction direction, Room[,] rooms)
        {
            Point towards = CurrentRoom.LocationOfRoom.Add(direction);
            if (towards.X < 0 || towards.X == Game.Width || towards.Y < 0 || towards.Y == Game.Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            }
            else if (CurrentRoom.CanGoThere[direction])
            {
                CurrentRoom = rooms[towards.X, towards.Y];
            }
            else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }
    }
}
