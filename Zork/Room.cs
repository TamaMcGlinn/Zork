using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Zork.Characters;
using Zork.Objects;
using Zork.UIContext;

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

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private Point _locationOfRoom;

        public Point LocationOfRoom
        {
            get { return _locationOfRoom; }
            set { _locationOfRoom = value; }
        }

        private List<BaseObject> _objectsInRoom = new List<BaseObject>();

        public List<BaseObject> ObjectsInRoom
        {
            get { return _objectsInRoom; }
            set { _objectsInRoom = value; }
        }

        private List<NPC> _charactersInRoom = new List<NPC>();

        public List<NPC> NPCsInRoom
        {
            get { return _charactersInRoom; }
        }
        #endregion properties

        public Room(string desc, Point locationOfRoom)
        {
            Description = desc;
            _canGoThere = new Dictionary<Direction, bool> {
                { Direction.North, false },
                { Direction.East, false },
                { Direction.South, false },
                { Direction.West, false }
            };
            LocationOfRoom = locationOfRoom;
        }

        public void PrintRoom() {
            using (new ColorContext(ColorContext.HeaderColor))
            {
                Console.WriteLine(Description + ". You can go:");
            }
            using (new ColorContext(ColorContext.DirectionsColor))
            {
                foreach (var kvp in CanGoThere)
                {
                    if (kvp.Value)
                    {
                        Console.WriteLine($"{kvp.Key}");
                    }
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Returns a string containing all the objects in the room
        /// </summary>
        /// <returns>A description of the objects in the room</returns>
        private void DescribeObjectsInRoom()
        {
            using (new ColorContext(ColorContext.HeaderColor))
            {
                Console.WriteLine("You see the following objects laying around:");
            }
            PrintItems(ObjectsInRoom);
        }

        public void PrintItems<T>(List<T> items) where T : BaseObject
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                using (new ColorContext(item.Color))
                {
                    ColorContext.PrintWithKeyCodes($"[{i + 1}] {item.Name} {item.Description}\n");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Returns a string containing all the characters in the room
        /// </summary>
        /// <returns>A description of the characters in the room</returns>
        public void PrintNPCs()
        {
            if(NPCsInRoom.Count == 0)
            {
                using (new ColorContext(ColorContext.FailureColor))
                {
                    Console.WriteLine("There's no one here.");
                }
            }
            for (int i = 0; i < NPCsInRoom.Count; i++)
            {
                string formattedName = NPCsInRoom[i].Name.Replace('_', ' ');
                ColorContext.PrintWithKeyCodes($"[{i+1}] {formattedName} : {NPCsInRoom[i].Description}\n");
            }
            Console.WriteLine();
        }

        public List<BaseObject> GetNonClueItems()
        {
            return ObjectsInRoom.Where(obj => !(obj is Clue)).ToList();
        }

        public void PrintRoomContents()
        {
            PrintNPCs();
            DescribeObjectsInRoom();
        }

        public NPC GetHostileNPC()
        {
            return NPCsInRoom.Find((NPC enemy) => enemy.IsHostile);
        }
    }
}
