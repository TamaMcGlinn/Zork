using System;
using System.Drawing;
using Zork.Behaviour;
using Zork.Characters;

namespace Zork
{
    public class Interactions
    {

        public void Battle(Maze maze, Room currentRoom, Player player)
        {
            NPC enemy = null;
            if (ChooseEnemyMessage(maze, currentRoom))
            {
                return;
            }
            int enemyNumber;
            if (int.TryParse(Console.ReadLine(), out enemyNumber) && enemyNumber > 0 && enemyNumber <= currentRoom.NPCsInRoom.Count)
            {
                enemy = currentRoom.NPCsInRoom[enemyNumber -1];
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
        public bool ChooseEnemyMessage(Maze maze, Room currentRoom)
        {
            if (currentRoom.NPCsInRoom.Count > 0)
            {
                for (int i = 0; i < currentRoom.NPCsInRoom.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {currentRoom.NPCsInRoom[i].Name}");
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
        /// Lists all items in the room and gives options for the player to pick them up. 
        /// If he chooses a valid item it gets added to the inventory.
        /// </summary>
        public void PickupItem(Maze maze, Point currentRoom, Player player)
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

        private void TryPickUp(Maze maze, Point currentRoom, int choiceIndex, Player player)
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
