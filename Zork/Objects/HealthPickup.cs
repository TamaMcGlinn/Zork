using System;

namespace Zork.Objects
{
    public class HealthPickup : BaseObject, IUseableObject
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

        public void UseObject(Character c)
        {
            c.Health = Math.Min(c.MaxHealth, c.Health + Potency);
            Console.WriteLine("Ahhh that's refreshing! Health:" + c.Health);
            c.Inventory.Remove(this);
        }
    }
}
