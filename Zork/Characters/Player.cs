using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;

namespace Zork.Characters
{
    public class Player : Character
    {
        #region properties
        private static readonly int MaxHealth = 100;

        private List<string> _cluesFound = new List<string>();

        public List<string> Clues
        {
            get { return _cluesFound; }
        }
        #endregion 

        public Player() : base()
        {
            Name = "Sherlock Holmes";
            Description = "A very good investigator.";
            EquippedWeapon = null;
            Strength = 10;
            Health = MaxHealth;
        }

        public void UseHealthPickup(HealthPickup h)
        {
            Health = Math.Min(MaxHealth, Health + h.Potency);
        }

        /// <summary>
        /// Attempt to go from the from point to the towards point.
        /// </summary>
        /// <param name="from">From point</param>
        /// <param name="towards">Destination point</param>
        /// <param name="direction">Direction from from to towards</param>
        public void TryGo(Maze maze, Point towards, Direction direction)
        {
            if (towards.X < 0 || towards.X == maze.Width || towards.Y < 0 || towards.Y == maze.Height)
            {
                Console.WriteLine("You attempt to go " + direction.ToString().ToLower() + " but face the end of the world.");
            }
            else if (maze[_location].CanGoThere[direction])
            {
                _location = towards;
            }
            else
            {
                Console.WriteLine("You cannot go " + direction.ToString().ToLower());
            }
        }
    }
}
