using System;

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

        public override ConsoleColor Colour => ConsoleColor.Magenta;
        #endregion

        public Weapon(string name, int strength, string description) : base(name,description)
        {
            Strength = strength;
        }

        public override void PickupObject(Character character)
        {
            character.EquippedWeapon = this;
        }

        public void PrintStats()
        {
            Console.WriteLine(Name + ": " + Description);
            Console.WriteLine("Strength: " + Strength);
        }
    }
}
