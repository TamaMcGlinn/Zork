using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;
using System.Drawing;

namespace Zork
{
    public class Character
    {

        #region properties
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _strength;

        public int Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        private int _health;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }


        private Weapon _weapon;

        public Weapon EquippedWeapon
        {
            get { return _weapon; }
            set { _weapon = value; }
        }

        private Room _location;

        public Room Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        #endregion properties

        /// <summary>
        /// Character constructor
        /// </summary>
        /// <param name="name">The name of the character</param>
        /// <param name="strength">The characters strength</param>
        /// <param name="health">The characters health</param>
        /// <param name="weapon">The weapon the character has equipped</param>
        /// <param name="location">Current location of the character</param>
        /// <param name="description">a description of what the character looks like</param>
        public Character(string name, int strength,int health, Weapon weapon, Room location, string description)
        {
            this.Name = name;
            this.Strength = strength;
            this.Health = health;
            this.EquippedWeapon = weapon;
            this.Location = location;
        }

        /// <summary>
        /// Creates a printable string of all things you see at your current location
        /// </summary>
        /// <returns>A string with all information about what you see</returns>
        public string lookAround()
        {
            StringBuilder sb = new StringBuilder();

            //prints the description of the room
            sb.AppendLine(Location.Description);

            //prints all characters
            sb.AppendLine("The following characters are in this room:");
            sb.Append(Location.printCharactersInRoom());

            //prints all objects
            sb.AppendLine("You see the following objects laying around:");
            sb.Append(Location.printObjectsInRoom());
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
    }
}
