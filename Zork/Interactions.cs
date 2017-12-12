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
            if (!ChooseEnemyMessage(maze, currentRoom))
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
                return true;
            }
            else
            {
                Console.WriteLine("\nThere's no one here\n");
                return false;
            }
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
    }
}
