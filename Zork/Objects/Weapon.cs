using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    public class Weapon : IObject
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
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion properties

        public Weapon(string name, int strength, string description)
        {
            Name = name;
            Strength = strength;
            Description = description;
        }


       
    }
}
