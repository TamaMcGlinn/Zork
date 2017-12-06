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

      public Player() : base()
       {
            Name = "Sherlock Holmes";
            Description = "A very good investigator.";
            EquippedWeapon = null;
            Strength = 10;
            Health = 100;
       }
    }
}
