using System;
using System.Collections.Generic;
using Zork.Objects;

namespace Zork.Characters
{
    public class Player : Character
    {
        #region properties
        
        public HashSet<string> Clues = new HashSet<string>();

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
