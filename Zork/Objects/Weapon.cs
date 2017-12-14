using System;

namespace Zork.Objects
{
    public class Weapon : UseableObject
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

        public override void PickupObject(Character character)
        {
            character.Inventory.Add(this);
        }

        public void PrintStats()
        {
            Console.WriteLine(Name + ": " + Description);
            Console.WriteLine("Strength: " + Strength);
        }

        public override void UseObject(Character c)
        {
            c.Inventory.Remove(this);
            if (c.EquippedWeapon != null)
            {
                c.Inventory.Add(c.EquippedWeapon);
            }
            c.EquippedWeapon = this;
            Console.WriteLine($"You've equipped a: {Name}");
        }
    }
}
