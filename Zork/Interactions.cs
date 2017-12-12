using System;
using System.Drawing;
using Zork.Behaviour;
using Zork.Characters;

namespace Zork
{
    public class Interactions
    {

    
      
    

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
