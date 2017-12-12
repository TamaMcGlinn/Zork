using System;
using System.Drawing;
using Zork.Behaviour;
using Zork.Characters;

namespace Zork
{
    public class Interactions
    {

        public void Battle(Maze maze, Point currentRoom, Player player)
        {
            Character enemy = null;
            if (ChooseEnemyMessage(maze, currentRoom))
            {
                return;
            }
            int enemyNumber;
            if (int.TryParse(Console.ReadLine(), out enemyNumber) && enemyNumber >= 0)
            {
                enemy = maze[currentRoom].CharactersInRoom[enemyNumber -1];
            }
            if (enemy != null)
            {
                BattleOutcomeEnum battleOutcome = player.Fight(enemy, maze.Rooms);
            }
        }
        /// <summary>
        /// Prints a list of characters you can fight and lets you choose a character
        /// </summary>
        /// <returns>Whether there are enemies in the current room</returns>
        public bool ChooseEnemyMessage(Maze maze, System.Drawing.Point currentRoom)
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
                return false;
            }
            return true;
        }
    

        /// <summary>
        /// Gets an enemy from the current room of the user depending on userinput
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="currentRoom"></param>
        /// <param name="enemyNumber"></param>
        /// <returns></returns>
        private Character getEnemyCharacterFromRoom(Maze maze, Point currentRoom, int enemyNumber)
        {
            if (enemyNumber >= 0 && enemyNumber < maze[currentRoom].CharactersInRoom.Count)
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
        /// Lists all items in the room and gives options for the player to pick them up. 
        /// If he chooses a valid item it gets added to the inventory.
        /// </summary>
        public static void PickupItem(Maze maze, Point currentRoom, Player player)
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
            TryPickUp(maze, currentRoom, inputInteger - 1, player);
        }

        private static void TryPickUp(Maze maze, Point currentRoom, int choiceIndex, Player player)
        {
            if (choiceIndex >= 0 && choiceIndex < maze[currentRoom].ObjectsInRoom.Count)
            {
                var obj = maze[currentRoom].ObjectsInRoom[choiceIndex];
                maze[currentRoom].ObjectsInRoom.Remove(obj);
                obj.PickupObject(player);
            }
            else
            {
                Console.WriteLine("Cannot pick that item up.");
            }
        }
    }
}
