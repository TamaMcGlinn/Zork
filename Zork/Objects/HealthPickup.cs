using System;

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

        public override ConsoleColor Colour => ConsoleColor.Red;

        public HealthPickup(string name, int potency, string description) : base(name, description)
        {
            _potency = potency;
        }
    }
}
