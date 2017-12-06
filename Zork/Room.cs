using System;
using System.Collections.Generic;
using System.Text;
using Zork.Objects;

namespace Zork
{
    public class Room
    {
        #region properties
        private Dictionary<Direction,bool> _canGoThere;

        public Dictionary<Direction,bool> CanGoThere
        {
            get { return _canGoThere; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private List<BaseObject> _objectsInRoom = new List<BaseObject>();

        public List<BaseObject> ObjectsInRoom
        {
            get { return _objectsInRoom; }
            set { _objectsInRoom = value; }
        }

        private List<Character> _charactersInRoom = new List<Character>();

        public List<Character> CharactersInRoom
        {
            get { return _charactersInRoom; }
        }
        #endregion properties

        public Room(string desc)
        {
            Description = desc;
            _canGoThere = new Dictionary<Direction, bool> {
                { Direction.North, false },
                { Direction.East, false },
                { Direction.South, false },
                { Direction.West, false }
            };
        }

        public void print() {
            Console.WriteLine(Description);
            Console.WriteLine("You can go:");
            foreach (var kvp in CanGoThere)
            {
                if (kvp.Value)
                {
                    Console.WriteLine(kvp.Key);
                }
            }
        }

        /// <summary>
        /// Prints a string containing all the objects in the room
        /// </summary>
        /// <returns>The string which is printed</returns>
        public string printObjectsInRoom()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ObjectsInRoom.Count; i++)
            {
                sb.Append(ObjectsInRoom[i].Name);
                sb.Append(" ");
                sb.AppendLine(ObjectsInRoom[i].Description);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Prints a string containing all the characters in the room
        /// </summary>
        /// <returns>The string which is printed</returns>
        public string printCharactersInRoom()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CharactersInRoom.Count; i++)
            {
                string formattedName = CharactersInRoom[i].Name.Replace('_', ' ');
                sb.Append(formattedName);
                sb.Append(" : ");
                sb.Append(CharactersInRoom[i].Description);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string LookAround()
        {
            StringBuilder sb = new StringBuilder();

            //prints the description of the room
            sb.AppendLine(Description);

            //prints all characters
            sb.AppendLine("The following characters are in this room:");
            sb.Append(printCharactersInRoom());

            //prints all objects
            sb.AppendLine("You see the following objects laying around:");
            sb.Append(printObjectsInRoom());
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

    }
}
