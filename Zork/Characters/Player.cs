using System;
using System.Collections.Generic;
using System.Drawing;
using Zork.Objects;
using Zork.UIContext;

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
            Console.WriteLine(CurrentRoom.DescribeRoom());
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
            if (enemy.EquippedWeapon != null)
            {
                Console.Write($" with his stupid {enemy.EquippedWeapon.Name} ");
            }
            return "";
        }

        /// <summary>
        /// Lists all items in the inventory
        /// </summary>
        public void PrintInventory()
        {
            PrintEquippedWeapon();
            if (Inventory.Count == 0)
            {
                using (new ColourContext(ColourContext.FailureColor))
                {
                    Console.WriteLine("You have no items in your inventory.\n");
                }
                return;
            }

            using (new ColourContext(ColourContext.HeaderColor))
            {
                Console.WriteLine("You currently have the following items:");
                for (int i = 0; i < Inventory.Count; i++)
                {
                    Console.ForegroundColor = Inventory[i].Colour;
                    Console.WriteLine($"{Inventory[i].Name} : {Inventory[i].Description}");
                }
                Console.WriteLine();
            }
        }

        public void PrintEquippedWeapon()
        {
            if (EquippedWeapon != null)
            {
                using (new ColourContext(EquippedWeapon.Colour))
                {
                    Console.WriteLine($"You're holding a {EquippedWeapon.Name} :  {EquippedWeapon.Description}");
                }
            }
            else
            {
                using (new ColourContext(ColourContext.FailureColor))
                {
                    Console.WriteLine("You're not holding a weapon");
                }
            }
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

        public void Battle(Game game)
        {
            Console.WriteLine(CurrentRoom.DescribeCharactersInRoom());
            if(CurrentRoom.NPCsInRoom.Count == 0)
            {
                return;
            }
            int enemyNumber;
            if (int.TryParse(Console.ReadLine(), out enemyNumber) && enemyNumber > 0 && enemyNumber <= CurrentRoom.NPCsInRoom.Count)
            {
                NPC enemy = CurrentRoom.NPCsInRoom[enemyNumber - 1];
                Fight(enemy, game);
            }
        }

        public void Fight(NPC enemy, Game game)
        {
            int turn = 0;
            while (enemy.Health > 0 && Health > 0)
            {                
                FightOneRound(enemy);
                ++turn;
                if (turn % enemy.LetsPlayerFleePerXRounds == 0 && AskFlee())
                {
                    Flee(game.maze);
                    return;
                }
            }
            CheckWhoWon(enemy);
            if (enemy.Health > 0)
            {
                enemy.KillThisNPC(game);
            }
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
        public void TryGo(Direction direction, Game game)
        {
            Point towards = CurrentRoom.LocationOfRoom.Add(direction);
            if (towards.X < 0 || towards.X == Game.Width || towards.Y < 0 || towards.Y == Game.Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            }
            else if (CurrentRoom.CanGoThere[direction])
            {
                CurrentRoom = game.maze[towards];
                foreach(NPC npc in game.NPCS)
                {
                    npc.OnPlayerMoved(game);
                }
            }
            else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }
    }
}
