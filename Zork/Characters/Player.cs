using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            using (new ColorContext(ColorContext.HeaderColor))
            {
                Console.WriteLine(CurrentRoom.Description);
            }
            CurrentRoom.PrintRoomContents();
        }

        public void UseHealthPickup(HealthPickup h)
        {
            h.UseObject(this);
        }

        private string PrintHittingWithWeapon()
        {
            if (EquippedWeapon != null)
            {
                return $" with your mighty {EquippedWeapon.Name}";
            }
            return "";
        }

        private string PrintGetHittedWithWeapon(NPC enemy)
        {
            if (enemy.EquippedWeapon != null)
            {
                return $" with his stupid {enemy.EquippedWeapon.Name}";
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
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("You have no items in your inventory.\n");
                }
                return;
            }

            using (new ColorContext(ColorContext.HeaderColor))
            {
                Console.WriteLine("You currently have the following items:");

                for (int i = 0; i < Inventory.Count; i++)
                {
                    using (new ColorContext(Inventory[i].Color))
                    {
                        Console.WriteLine($"{Inventory[i].Name} {Inventory[i].Description}");
                    }
                }
                Console.WriteLine();
            }
        }

        public void PrintEquippedWeapon()
        {
            if (EquippedWeapon != null)
            {
                using (new ColorContext(EquippedWeapon.Color))
                {
                    Console.WriteLine($"You're holding a {EquippedWeapon.Name} :  {EquippedWeapon.Description}");
                }
            }
            else
            {
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("You're not holding a weapon");
                }
            }
        }

        protected override void FightOneRound(NPC enemy)
        {
            int enemyDamage = enemy.GenerateDamage();
            int myDamage = GenerateDamage();

            enemy.TakeDamage(myDamage);
            TakeDamage(enemyDamage);

            using (new ColorContext(ColorContext.BattleHit))
            {
                Console.Write("You hit for " + myDamage + " damage" + PrintHittingWithWeapon() + ".");
            }
            using (new ColorContext(ColorContext.BattleDamage))
            {
                Console.Write($"\n{enemy.Name} hits you for " + enemyDamage + " damage" + PrintGetHittedWithWeapon(enemy) + ".");
            }
            Console.WriteLine($"\nYou have {Health} hp left, he has {enemy.Health} hp left.");
        }
        
        public void UseObject()
        {
            //check for useable items and display a list of which to use, otherwise show a non item available message
            List<UseableObject> useables = GetUseables();
            if (useables.Count > 0)
            {
                Console.WriteLine("Choose which item you would like to use:");
                CurrentRoom.PrintItems(useables);
                ChooseObjectToUse(useables);
            }
            else
            {
                Console.WriteLine("You have no useable items in your inventory.");
            }
        }

        private List<UseableObject> GetUseables()
        {
            return Inventory.Where(x => x is UseableObject).Cast<UseableObject>().ToList();
        }

        private void ChooseObjectToUse(List<UseableObject> useableItems)
        {
            string userInput = Console.ReadLine();

            if (userInput.Length <= 0)
            {
                return;
            }
            int userInputNumber;
            int.TryParse(userInput, out userInputNumber);
            userInputNumber--;
            if (userInputNumber >= 0 && userInputNumber < useableItems.Count)
            {
                useableItems[userInputNumber].UseObject(this);
            }
        }

        public void Battle(Game game)
        {
            CurrentRoom.PrintNPCs();
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
                if (UserFlees(enemy, turn))
                {
                    Flee(game.maze);
                    return;
                }
            }
            CheckWhoWon(enemy, game);
        }

        private bool UserFlees(NPC enemy, int turn)
        {
            return enemy.Health > 0 && Health > 0 && turn % enemy.LetsPlayerFleePerXRounds == 0 && AskFlee();
        }

        protected bool AskFlee()
        {
            Console.WriteLine("Do you want to flee? Y/N");
            string userInput = Console.ReadLine();
            userInput = userInput.ToLower();
            if ( userInput.Length != 1)
            {
                return false;
            }
            return (userInput[0] == 'y');
        }

        /// <summary>
        /// Lets your character run to a random room
        /// </summary>
        /// <param name="allRooms"></param>
        public void Flee(Maze maze)
        {
            CurrentRoom = maze.GetRandomOtherRoom(CurrentRoom);
            Console.WriteLine("When danger reared its ugly head,\nyou bravely turned your tail and fled...");
        }
        
        #region talkMethods

        public void TryTalk()
        {
            if(CurrentRoom.NPCsInRoom.Count == 0)
            {
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("There's no one in the room.");
                }
                return;
            }
            Console.WriteLine("With whom do you want to talk?");
            CurrentRoom.PrintNPCs();
            int talkToNPCInt;
            int.TryParse(Console.ReadLine(), out talkToNPCInt);
            talkToNPCInt -= 1;
            if(talkToNPCInt >= 0 && talkToNPCInt <= CurrentRoom.NPCsInRoom.Count)
            {
                CurrentRoom.NPCsInRoom[talkToNPCInt].Talk(this);
            }
            else
            {
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("He's not here...");
                }
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
            if (towards.OutOfBounds())
            {
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
                }
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
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("You cannot go " + direction.ToString().ToLower());
                }
            }
        }
    }
}
