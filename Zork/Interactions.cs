using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Characters;

namespace Zork
{
    public static class Interactions
    {

        public static void Battle(Maze maze, Point currentRoom)
        {
            Character enemy = null;
            Interactions.ChooseEnemyMessage(maze, currentRoom);
            int enemyNumber;
            if (int.TryParse(Console.ReadLine(), out enemyNumber) && enemyNumber >= 0)
            {
                enemy = maze[currentRoom].CharactersInRoom[enemyNumber];
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
            if (maze[currentRoom].CharactersInRoom.Count > 0)
            {
                for (int i = 0; i < maze[currentRoom].CharactersInRoom.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {maze[currentRoom].CharactersInRoom[i].Name}");
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
            if (enemyNumber >= 0)
            {
                if (enemyNumber < maze[currentRoom].CharactersInRoom.Count + 1)
                {
                    return maze[currentRoom].CharactersInRoom[enemyNumber - 1];
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
        /// <param name="enemy"></param>
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
            if (inputInteger > 0 && inputInteger < maze[currentRoom].ObjectsInRoom.Count)
            {
                (maze[currentRoom].ObjectsInRoom[inputInteger - 1]).PickupObject(Characters.CharacterDefinitions.PlayerCharacter);
            }
            else
            {
                Console.WriteLine("Cannot pick that item up.");
            }
        }
        public static int ReadUserInputInteger()
        {
            int userInput;
            int.TryParse(Console.ReadLine(), out userInput);
            return userInput;
        }
    }
}
