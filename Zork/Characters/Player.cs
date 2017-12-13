using System;
using System.Collections.Generic;
using System.Drawing;
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
            CurrentRoom.PrintLookAroundString();
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

        private string PrintGetHittedWithWeapon(NPC enemy)
        {
            if (EquippedWeapon != null)
            {
                Console.Write($" with his stupid {enemy.EquippedWeapon.Name} ");
            }
            return "";
        }

        protected override void FightOneRound(NPC enemy)
        {
            int enemyDamage = enemy.GenerateDamage();
            int myDamage = GenerateDamage();

            Console.WriteLine("You hit eachother...");
            enemy.TakeDamage(myDamage);
            TakeDamage(enemyDamage);

            Console.Write("You hit for: " + myDamage + PrintHittingWithWeapon());
            Console.Write($"\n{enemy.Name} hits you for:" + enemyDamage + PrintGetHittedWithWeapon(enemy));
            Console.WriteLine($"\nYou have {Health} hp left, he has {enemy.Health} hp left.");
        }
        public void Battle(Maze maze)
        {
            if (!CurrentRoom.PrintAvailableEnemiesInRoom())
            {
                return;
            }
            int enemyNumber;
            if (int.TryParse(Console.ReadLine(), out enemyNumber) && enemyNumber > 0 && enemyNumber <= CurrentRoom.NPCsInRoom.Count)
            {
                NPC enemy = CurrentRoom.NPCsInRoom[enemyNumber - 1];
                Fight(enemy, maze);
            }
        }

        public void Fight(NPC enemy, Maze maze)
        {
            int turn = 0;
            while (enemy.Health > 0 && Health > 0)
            {                
                FightOneRound(enemy);
                ++turn;
                if (turn % enemy.LetsPlayerFleePerXRounds == 0 && AskFlee())
                {
                    Flee(maze);
                    return;
                }
            }
            CheckWhoWon(enemy);
        }

        protected bool AskFlee()
        {
            Console.WriteLine("Do you want to flee? Y/N");
            char userInputCharacter = (char)Console.Read();
            return (userInputCharacter == 'y' || userInputCharacter == 'Y');
        }

        /// <summary>
        /// Lets your character run to a random room
        /// </summary>
        /// <param name="allRooms"></param>
        public void Flee(Maze maze)
        {
            CurrentRoom = maze.GetRandomOtherRoom(CurrentRoom);
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
