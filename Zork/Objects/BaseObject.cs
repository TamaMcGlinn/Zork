using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    public abstract class BaseObject
    {
        #region properties
        private string _name;

        public string Name
        {
            get { return _name; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
        }
        #endregion properties

        public BaseObject(string name, string description)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Default puts the object in the specified character inventory.
        /// </summary>
        /// <param name="character"></param>
        public virtual void PickupObject(Character character)
        {
            character.Inventory.Add(this);
        }
    }
}
