using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    public class Weapon : BaseObject
    {
        #region properties
        private int _strength;

        public int Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }
        #endregion

        public Weapon(string name, int strength, string description) : base(name,description)
        {
            Strength = strength;
        }


       
    }
}
