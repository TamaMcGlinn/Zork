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

        public Player() : base("Sherlock Holmes", "A very good investigator.", 10, 100)
        {
        }

        public void UseHealthPickup(HealthPickup h)
        {
            Health = Math.Min(MaxHealth, Health + h.Potency);
        }
    }
}
