using System;
using System.Collections.Generic;
using System.Text;
using Zork.Objects;

namespace Zork
{
    public class Room : IObject
    {
        #region properties
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

        private List<IObject> _objectsInRoom = new List<IObject>();

        public List<IObject> ObjectsInRoom
        {
            get { return _objectsInRoom; }
            set { _objectsInRoom = value; }
        }

        private List<Character> _charactersInRoom = new List<Character>();

        public List<Character> CharactersInRoom
        {
            get { return _charactersInRoom; }
            set { _charactersInRoom = value; }
        }
        #endregion properties

        public Room(string desc)
        {
            Description = desc;
        }

        public void Print() {
            Console.WriteLine(Description);
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
                sb.Append(CharactersInRoom[i].Name);
                sb.Append(" : ");
                sb.Append(CharactersInRoom[i].Description);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
