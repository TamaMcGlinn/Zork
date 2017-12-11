using System;
using System.Drawing;
using Zork.Characters;

namespace Zork
{
    public static class Interactions
    {

        public static void Battle(Maze maze, Point currentRoom)
        {
            Character enemy = null;
            ChooseEnemyMessage(maze, currentRoom);
            int enemyNumber;
            Room theRoom = maze[currentRoom];
            if (int.TryParse(Console.ReadLine(), out enemyNumber) && enemyNumber > 0 && enemyNumber <= theRoom.NPCsInRoom.Count)
            {
                enemy = theRoom.NPCsInRoom[enemyNumber - 1];
            }
            if (enemy != null)
            {
                Interactions.Fight(enemy, CharacterDefinitions.PlayerCharacter);
            }
        }
        /// <summary>
        /// Prints a list of characters you can fight and lets you choose a character
        /// </summary>
        public static void ChooseEnemyMessage(Maze maze, System.Drawing.Point currentRoom)
        {
            if (maze[currentRoom].NPCsInRoom.Count > 0)
            {
                for (int i = 0; i < maze[currentRoom].NPCsInRoom.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {maze[currentRoom].NPCsInRoom[i].Name}");
                }
            }
            else
            {
                Console.WriteLine("\nThere's no one here\n");

            }
        }
    

        /// <summary>
        /// Gets an enemy from the current room of the user depending on userinput
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="currentRoom"></param>
        /// <param name="enemyNumber"></param>
        /// <returns></returns>
        private static Character getEnemyCharacterFromRoom(Maze maze, Point currentRoom, int enemyNumber)
        {
            if (enemyNumber >= 0 && enemyNumber < maze[currentRoom].NPCsInRoom.Count)
            {
                if (enemyNumber < maze[currentRoom].NPCsInRoom.Count + 1)
                {
                    return maze[currentRoom].NPCsInRoom[enemyNumber - 1];
                }
                else
                {
                    Console.WriteLine("That enemy is not available.");
                }
            }
            return null;
        }

        /// <summary>
        /// Fights the chosen enemy untill someone dies, if player dies he loses all his items, 
        /// if enemy dies player picks up all his items.
        /// </summary>
        /// <param name="enemy">The character you wish to fight</param>
        /// <param name="player"></param>
        /// <returns>A boolean indicating wether the player won the fight</returns>
        public static bool Fight(Character enemy, Character player)
        {
            while (player.Health > 0 && enemy.Health > 0)
            {
                FightOneRound(enemy, player);
            }
            return CheckWhoWon(enemy, player);
        }

        private static bool CheckWhoWon(Character enemy, Character player)
        {
            if (player.Health < 0)
            {
                player.Inventory.Clear();
                player.ResetHealth();
                Console.WriteLine("You died! But luckily you've returned without items.");
                return false;
            }
            else
            {
                player.Inventory.AddRange(enemy.Inventory);
                Console.WriteLine($"You've won! You've picked up all {enemy.Name}'s items, check your inventory!");
                return true;
            }
        }

        private static void FightOneRound(Character enemy, Character player)
        {
            int playerDamage = GenerateDamage(player);
            int enemyDamage = GenerateDamage(enemy);

            Console.WriteLine("You hit eachother...");
            player.TakeDamage(enemyDamage);
            enemy.TakeDamage(playerDamage);

            Console.Write("You hit for: " + playerDamage + GetPlayerWeaponString(player));
            Console.Write($"\n{enemy.Name} hits you for:" + enemyDamage + GetEnemyWeaponString(enemy));
            Console.WriteLine($"\nYou have {player.Health} hp left, he has {enemy.Health} hp left.");
        }

        private static string GetPlayerWeaponString(Character player)
        {
            if (player.EquippedWeapon != null)
            {
                Console.Write($" with your mighty {player.EquippedWeapon.Name}");
            }
            return "";
        }

        private static string GetEnemyWeaponString(Character player)
        {
            if (player.EquippedWeapon != null)
            {
                Console.Write($" with his stupid {player.EquippedWeapon.Name}");
            }
            return "";
        }

        private static int GenerateDamage(Character player)
        {
            Random turnBonusDamageGenerator = new Random();
            const int maxBonusDamage = 10;
            int bonusDamage = turnBonusDamageGenerator.Next(0, maxBonusDamage);

            int playerDamage = player.Strength + bonusDamage;
            if (player.EquippedWeapon != null)
            {
                playerDamage += player.EquippedWeapon.Strength;
            }
            return playerDamage;
        }

        /// <summary>
        /// Lists all items in the room and gives options for the player to pick them up. 
        /// If he chooses a valid item it gets added to the inventory.
        /// </summary>
        public static void PickupItem(Maze maze, Point currentRoom)
        {
            if (maze[currentRoom].ObjectsInRoom.Count <= 0)
            {
                Console.WriteLine("There are no items to pickup in this room.");
                return;
            }
            for (int i = 0; i < maze[currentRoom].ObjectsInRoom.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] to pickup:" + maze[currentRoom].ObjectsInRoom[i].Name);
            }
            string input = Console.ReadLine();
            int inputInteger;
            int.TryParse(input, out inputInteger);
            TryPickUp(maze, currentRoom, inputInteger - 1);
        }

        private static void TryPickUp(Maze maze, Point currentRoom, int choiceIndex)
        {
            if (choiceIndex >= 0 && choiceIndex < maze[currentRoom].ObjectsInRoom.Count)
            {
                var obj = maze[currentRoom].ObjectsInRoom[choiceIndex];
                maze[currentRoom].ObjectsInRoom.Remove(obj);
                obj.PickupObject(CharacterDefinitions.PlayerCharacter);
            }
            else
            {
                Console.WriteLine("Cannot pick that item up.");
            }
        }
    }
}
