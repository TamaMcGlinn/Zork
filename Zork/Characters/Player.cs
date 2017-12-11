using System;
using System.Collections.Generic;
using Zork.Objects;

namespace Zork.Characters
{
    public class Player : Character
    {
        #region properties

        private List<string> _cluesFound = new List<string>();

        public List<string> Clues
        {
            get { return _cluesFound; }
        }




        #endregion

        public Player() : base()
        {
            Name = "Sherlock Holmes";
            Description = "A very good investigator.";
            EquippedWeapon = null;
            Strength = 10;
            Health = MaxHealth;
        }

        public void UseHealthPickup(HealthPickup h)
        {
            Health = Math.Min(MaxHealth, Health + h.Potency);
        }
    }
}
