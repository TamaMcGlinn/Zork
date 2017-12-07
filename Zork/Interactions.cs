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
    public class Interactions
    {
        /// <summary>
        /// Prints a list of characters you can fight and lets you choose a character
        /// </summary>
        public Character ChooseEnemy(Maze maze, System.Drawing.Point currentRoom)
        {
            for (int i = 0; i < maze[currentRoom].CharactersInRoom.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {maze[currentRoom].CharactersInRoom[i].Name}");
            }

            int enemyNumber = ReadUserInputInteger();
            return getEnemyCharacterFromRoom(maze, currentRoom, enemyNumber); ;
        }


        private Character getEnemyCharacterFromRoom(Maze maze, Point currentRoom, int enemyNumber)
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
        /// Fights the chosen enemy untill someone dies, if player dies he loses all his items, if enemy dies player picks up all his items.
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns>A boolean indicating wether the player won the fight</returns>
        public bool Fight(Character enemy, Character player)
        {
            Random turnBonusDamageGenerator = new Random();
            int maxBonusDamage = 10;

            while (player.Health > 0 && enemy.Health > 0)
            {
                int playerBonusDamage = turnBonusDamageGenerator.Next(0, maxBonusDamage);
                int enemyBonusDamage = turnBonusDamageGenerator.Next(0, maxBonusDamage);

                int playerDamage = playerBonusDamage + player.Strength;
                int enemyDamage = enemyBonusDamage + enemy.Strength;

                if (player.EquippedWeapon != null)
                {
                    playerDamage += player.EquippedWeapon.Strength;
                }
                if (enemy.EquippedWeapon != null)
                {
                    enemyDamage += enemy.EquippedWeapon.Strength;
                }

                Console.WriteLine("You hit eachother...");
                player.TakeDamage(enemyDamage);
                enemy.TakeDamage(playerDamage);


                Console.Write("You hit for: " + playerDamage);
                if (player.EquippedWeapon != null)
                {
                    Console.Write($" with you mighty {player.EquippedWeapon.Name}");
                }
                Console.WriteLine();
                Console.Write($"{enemy.Name} hits you for:" + enemyDamage);
                if (enemy.EquippedWeapon != null)
                {
                    Console.Write($" with his stupid {enemy.EquippedWeapon}");
                }
                Console.WriteLine();
                Console.WriteLine($"You have {player.Health} hp left, he has {enemy.Health} hp left.");
            }
            if (player.Health < 0)
            {
                player.Inventory.Clear();
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

        /// <summary>
        /// Lists all items in the room and gives options for the player to pick them up. 
        /// If he chooses a valid item it gets added to the inventory.
        /// </summary>
        public void PickupItem(Maze maze, Point currentRoom)
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

        public int ReadUserInputInteger()
        {
            int userInput;
            int.TryParse(Console.ReadLine(), out userInput);
            return userInput;
        }


    }
}
