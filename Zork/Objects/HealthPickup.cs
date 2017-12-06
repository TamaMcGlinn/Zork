using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    public class HealthPickup : BaseObject
    {
        private int _potency;

        /// <summary>
        /// How many hitpoints are restored upon usage.
        /// </summary>
        public int Potency
        {
            get { return _potency; }
        }

        public HealthPickup(string name, int potency, string description) : base(name, description)
        {
            _potency = potency;
        }
    }
}
