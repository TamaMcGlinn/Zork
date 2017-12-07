using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;

namespace Zork.Characters
{
    public class Player : Character
    {
        private static readonly int MaxHealth = 100;

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
